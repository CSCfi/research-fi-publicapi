using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface.Tests.TestHelpers;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Moq;
using Nest;
using Xunit;
using HttpMethod = Elasticsearch.Net.HttpMethod;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace CSC.PublicApi.Interface.Tests.Services;

public class ElasticSearchIndexBoundaryTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1499)]
    [InlineData(1500)]
    [InlineData(1501)]
    public async Task IndexAsync_BatchesEveryDocumentExactlyOnceBeforeSwitching(int count)
    {
        var scenario = new Scenario();

        await scenario.Service.IndexAsync("synthetic", Documents(count), typeof(TestModel));

        var calls = scenario.Requests();
        var expected = new List<string> { "HEAD /synthetic_v1", "DELETE /synthetic_v1", "PUT /synthetic_v1" };
        expected.AddRange(Enumerable.Repeat("POST /synthetic_v1/_bulk", (count + 1499) / 1500));
        expected.AddRange(new[] { "GET /_cluster/health/synthetic_v1", "POST /_aliases", "DELETE /synthetic_v2" });
        Assert.Equal(expected, calls.Select(Key));
        AssertBulkDocuments(scenario, count);
        Assert.All(calls, call => Assert.True(call.Success));
        var health = calls.Single(call => call.Uri.AbsolutePath.StartsWith("/_cluster/health/", StringComparison.Ordinal));
        Assert.Contains("wait_for_status=yellow", health.Uri.Query);
        Assert.Contains("wait_for_active_shards=1", health.Uri.Query);
        using var aliases = JsonDocument.Parse(calls.Single(call => call.Uri.AbsolutePath == "/_aliases").RequestBodyInBytes!);
        var actions = aliases.RootElement.GetProperty("actions");
        Assert.Equal(2, actions.GetArrayLength());
        Assert.Equal("*", actions[0].GetProperty("remove").GetProperty("index").GetString());
        Assert.Equal("synthetic", actions[0].GetProperty("remove").GetProperty("alias").GetString());
        Assert.Equal("synthetic_v1", actions[1].GetProperty("add").GetProperty("index").GetString());
        Assert.Equal("synthetic", actions[1].GetProperty("add").GetProperty("alias").GetString());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task IndexChunkAsync_BulkFailureStopsLaterBatchesAndLogs(bool itemFailure)
    {
        var scenario = new Scenario(HttpMethod.POST, "/synthetic_v1/_bulk",
            itemFailure ? ItemError : Error, itemFailure ? 200 : 503);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            scenario.Service.IndexChunkAsync("synthetic_v1", Documents(1501), typeof(TestModel)));

        Assert.Equal(new[] { "POST /synthetic_v1/_bulk" }, scenario.Requests().Select(Key));
        Assert.Equal(1, scenario.InjectedResponses);
        Assert.Contains(scenario.Errors, message => message.Contains("failed on batch", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1499)]
    [InlineData(1500)]
    [InlineData(1501)]
    public async Task IndexChunkAsync_OnlyWritesBatchesWithoutActivatingIndex(int count)
    {
        var scenario = new Scenario();

        await scenario.Service.IndexChunkAsync("synthetic_v1", Documents(count), typeof(TestModel));

        Assert.Equal(Enumerable.Repeat("POST /synthetic_v1/_bulk", (count + 1499) / 1500),
            scenario.Requests().Select(Key));
        AssertBulkDocuments(scenario, count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \t")]
    public async Task IndexAsync_RejectsMissingIndexBeforeAnyRequest(string? name)
    {
        var scenario = new Scenario();

        var error = await Assert.ThrowsAsync<ArgumentException>(() =>
            scenario.Service.IndexAsync(name!, Documents(1), typeof(TestModel)));

        Assert.Equal("indexName", error.ParamName);
        Assert.Empty(scenario.Calls);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task IndexAsync_CreateFailureStopsBeforeBulkAndActivation(bool unacknowledged)
    {
        var scenario = new Scenario(HttpMethod.PUT, "/synthetic_v1",
            unacknowledged ? """{"acknowledged":false,"shards_acknowledged":false}""" : Error,
            unacknowledged ? 200 : 400);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            scenario.Service.IndexAsync("synthetic", Documents(1), typeof(TestModel)));

        Assert.Equal(new[] { "HEAD /synthetic_v1", "DELETE /synthetic_v1", "PUT /synthetic_v1" },
            scenario.Requests().Select(Key));
        Assert.Equal(1, scenario.InjectedResponses);
    }

    [Theory]
    [InlineData(false, 1)]
    [InlineData(false, 2)]
    [InlineData(true, 1)]
    [InlineData(true, 2)]
    public async Task IndexAsync_BulkFailureLogsAndPreventsFurtherBatchesOrActivation(bool itemFailure, int batch)
    {
        var scenario = new Scenario(HttpMethod.POST, "/synthetic_v1/_bulk",
            itemFailure ? ItemError : Error, itemFailure ? 200 : 503, batch);

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            scenario.Service.IndexAsync("synthetic", Documents(3001), typeof(TestModel)));

        Assert.Contains("Indexing documents to synthetic_v1 failed", error.Message);
        var expected = new List<string> { "HEAD /synthetic_v1", "DELETE /synthetic_v1", "PUT /synthetic_v1" };
        expected.AddRange(Enumerable.Repeat("POST /synthetic_v1/_bulk", batch));
        Assert.Equal(expected, scenario.Requests().Select(Key));
        Assert.Equal(1, scenario.InjectedResponses);
        Assert.Contains(scenario.Errors, message => message.Contains("failed on batch", StringComparison.Ordinal));
        if (itemFailure)
            Assert.Contains(scenario.Errors, message => message.Contains("Synthetic item rejected", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("/_cluster/health/synthetic_v1", 503, false)]
    [InlineData("/_cluster/health/synthetic_v1", 200, false)]
    [InlineData("/_aliases", 503, false)]
    [InlineData("/_aliases", 200, false)]
    [InlineData("/synthetic_v2", 503, false)]
    [InlineData("/synthetic_v2", 404, true)]
    public async Task SwitchIndexes_CurrentlyContinuesAfterUncheckedResponses(string path, int status, bool allowedMissing)
    {
        // P4-ROTATE: characterization of unsafe transitions, not an approved success contract.
        var body = status >= 400 ? Error : path == "/_aliases"
            ? """{"acknowledged":false}"""
            : """{"status":"red","timed_out":true,"number_of_nodes":1,"active_primary_shards":0}""";
        var method = path.StartsWith("/_cluster/", StringComparison.Ordinal) ? HttpMethod.GET :
            path == "/_aliases" ? HttpMethod.POST : HttpMethod.DELETE;
        var scenario = new Scenario(method, path, body, status);

        await scenario.Service.SwitchIndexes("synthetic", "synthetic_v1", "synthetic_v2", nameof(TestModel));

        Assert.Equal(new[] { "GET /_cluster/health/synthetic_v1", "POST /_aliases", "DELETE /synthetic_v2" },
            scenario.Requests().Select(Key));
        Assert.Equal(1, scenario.InjectedResponses);
        Assert.Empty(scenario.Errors);
        if (!allowedMissing && status >= 400)
            Assert.False(scenario.Requests().Single(call => call.Uri.AbsolutePath == path).Success);
    }

    [Fact]
    public async Task CreateIndex_CurrentlyAttemptsCreationAfterFailedPreDelete()
    {
        var scenario = new Scenario(HttpMethod.DELETE, "/synthetic_v1", Error, 503);

        await scenario.Service.CreateIndex("synthetic_v1", typeof(TestModel));

        Assert.Equal(new[] { "DELETE /synthetic_v1", "PUT /synthetic_v1" }, scenario.Requests().Select(Key));
        Assert.Equal(1, scenario.InjectedResponses);
        Assert.Empty(scenario.Errors);
    }

    [Fact]
    public async Task GetIndexNames_CurrentlyTreatsFailedExistenceCheckAsMissing()
    {
        var scenario = new Scenario(HttpMethod.HEAD, "/synthetic_v1", Error, 503);

        var (create, delete) = await scenario.Service.GetIndexNames("synthetic");

        Assert.Equal("synthetic_v1", create);
        Assert.Equal("synthetic_v2", delete);
        Assert.Equal(new[] { "HEAD /synthetic_v1" }, scenario.Requests().Select(Key));
        Assert.Equal(1, scenario.InjectedResponses);
        Assert.Empty(scenario.Errors);
    }

    private static List<object> Documents(int count) =>
        Enumerable.Range(0, count).Select(id => (object)new TestModel { Id = $"synthetic-{id}" }).ToList();

    private static string Key(IApiCallDetails call) => $"{call.HttpMethod} {call.Uri.AbsolutePath}";

    private static void AssertBulkDocuments(Scenario scenario, int count)
    {
        var bulk = scenario.Requests().Where(call => call.Uri.AbsolutePath.EndsWith("/_bulk", StringComparison.Ordinal)).ToArray();
        var ids = new List<string?>();
        for (var batch = 0; batch < bulk.Length; batch++)
        {
            var lines = Encoding.UTF8.GetString(bulk[batch].RequestBodyInBytes!)
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2 * Math.Min(1500, count - batch * 1500), lines.Length);
            for (var line = 0; line < lines.Length; line += 2)
            {
                using var action = JsonDocument.Parse(lines[line]);
                Assert.True(action.RootElement.TryGetProperty("index", out _));
                using var document = JsonDocument.Parse(lines[line + 1]);
                ids.Add(document.RootElement.GetProperty("Id").GetString());
            }
        }
        Assert.Equal(Enumerable.Range(0, count).Select(id => $"synthetic-{id}"), ids);
        Assert.Equal(count, ids.Distinct().Count());
    }

    private const string Error = """{"error":{"type":"synthetic_failure","reason":"Synthetic request failed"},"status":503}""";
    private const string ItemError = """{"took":0,"errors":true,"items":[{"index":{"_index":"synthetic_v1","_type":"_doc","_id":"synthetic-rejected","status":400,"error":{"type":"mapper_parsing_exception","reason":"Synthetic item rejected"}}}]}""";

    private sealed class Scenario
    {
        private readonly Connection _connection;
        public List<IApiCallDetails> Calls { get; } = new();
        public List<string> Errors { get; } = new();
        public ElasticSearchIndexService Service { get; }
        public int InjectedResponses => _connection.InjectedResponses;

        public Scenario(HttpMethod? method = null, string? path = null, string? body = null, int status = 200, int occurrence = 1)
        {
            _connection = new Connection(method, path, body, status, occurrence);
            var client = new ElasticClient(new ConnectionSettings(
                    new SingleNodeConnectionPool(new Uri("http://elasticsearch.test:9200")), _connection)
                .DefaultFieldNameInferrer(name => name).MaximumRetries(0)
                .DisableDirectStreaming().OnRequestCompleted(Calls.Add));
            var logger = new Mock<ILogger<ElasticSearchIndexService>>();
            logger.Setup(log => log.Log(
                    LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception?>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
                .Callback(new InvocationAction(invocation => Errors.Add(invocation.Arguments[2].ToString()!)));
            Service = new ElasticSearchIndexService(client, logger.Object);
        }

        public IApiCallDetails[] Requests() => Calls.Where(call => call.Uri.AbsolutePath != "/").ToArray();
    }

    private sealed class Connection(HttpMethod? method, string? path, string? body, int status, int occurrence)
        : MockElasticConnection
    {
        private int _matches;
        public int InjectedResponses { get; private set; }

        private (byte[] Body, int Status)? Response(RequestData request)
        {
            if (request.Method == method && request.Uri.AbsolutePath == path && ++_matches == occurrence)
            {
                InjectedResponses++;
                var responseBody = body == Error ? JsonSerializer.Serialize(new
                {
                    error = new { type = "synthetic_failure", reason = "Synthetic request failed" },
                    status
                }) : body!;
                return (Encoding.UTF8.GetBytes(responseBody), status);
            }
            if (request.Method != HttpMethod.POST || !request.Uri.AbsolutePath.EndsWith("/_bulk", StringComparison.Ordinal))
                return null;
            using var stream = new MemoryStream();
            request.PostData.Write(stream, request.ConnectionSettings);
            var lines = Encoding.UTF8.GetString(stream.ToArray()).Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var items = Enumerable.Range(0, lines.Length / 2).Select(id => new
            {
                index = new { _index = "synthetic_v1", _type = "_doc", _id = $"document-{id}", _version = 1, status = 201, result = "created" }
            });
            return (JsonSerializer.SerializeToUtf8Bytes(new { took = 0, errors = false, items }), 200);
        }

        public override TResponse Request<TResponse>(RequestData requestData)
        {
            var response = Response(requestData);
            return response.HasValue ? ReturnConnectionStatus<TResponse>(requestData, response.Value.Body,
                response.Value.Status, RequestData.DefaultJsonMimeType) : base.Request<TResponse>(requestData);
        }

        public override Task<TResponse> RequestAsync<TResponse>(RequestData requestData, CancellationToken cancellationToken)
        {
            var response = Response(requestData);
            return response.HasValue ? ReturnConnectionStatusAsync<TResponse>(requestData, cancellationToken,
                response.Value.Body, response.Value.Status, RequestData.DefaultJsonMimeType) :
                base.RequestAsync<TResponse>(requestData, cancellationToken);
        }
    }
}
