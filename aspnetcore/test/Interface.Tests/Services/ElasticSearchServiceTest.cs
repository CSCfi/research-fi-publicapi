using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Tests.TestHelpers;
using Elasticsearch.Net;
using Nest;
using Xunit;
using Publication = CSC.PublicApi.Service.Models.Publication.Publication;

namespace CSC.PublicApi.Interface.Tests.Services;

public class ElasticSearchServiceTest
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 11, 2)]
    [InlineData(3, 21, 3)]
    public async Task Search_MapsDocumentsAndPagination(int hits, long total, int pages)
    {
        var scenario = new Scenario(new Step("_search", SearchResponse(total,
            Enumerable.Range(1, hits).Select(id => (long)id).ToArray())));

        var (documents, result) = await scenario.Service.Search(Parameters(), 2, 10);

        Assert.Equal(Enumerable.Range(1, hits).Select(id => $"publication-{id}"), documents.Select(item => item.Id));
        Assert.All(documents, item => Assert.Equal("Synthetic publication", item.Name));
        Assert.Equal(2, result.PageNumber);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(total, result.TotalResults);
        Assert.Equal(pages, result.TotalPages);
        var request = scenario.AssertRequests("_search").Single();
        Assert.Equal(10, request.GetProperty("from").GetInt32());
        Assert.Equal(10, request.GetProperty("size").GetInt32());
        AssertNameFilter(request);
    }

    [Fact]
    public async Task Search_NoHitsMetadataReturnsEmptyPageWithZeroTotal()
    {
        var scenario = new Scenario(new Step("_search", """{"took":0,"timed_out":false,"_shards":{"total":1,"successful":1,"failed":0}}"""));

        var (documents, result) = await scenario.Service.Search(Parameters(), 1, 5);

        Assert.Empty(documents);
        Assert.Equal(0, result.TotalResults);
        Assert.Equal(0, result.TotalPages);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(5, result.PageSize);
        scenario.AssertRequests("_search");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetSingle_ReturnsFirstDocumentOrNull(int hits)
    {
        var scenario = new Scenario(new Step("_search", SearchResponse(hits,
            Enumerable.Range(1, hits).Select(id => (long)id).ToArray())));

        var document = await scenario.Service.GetSingle("public-id-not-document-id");

        if (hits == 0)
            Assert.Null(document);
        else
        {
            Assert.NotNull(document);
            Assert.Equal("publication-1", document!.Id);
            Assert.Equal("Synthetic publication", document.Name);
        }
        var request = scenario.AssertRequests("_search").Single();
        Assert.Equal("public-id-not-document-id", request.GetProperty("query").GetProperty("term")
            .GetProperty("Id").GetProperty("value").GetString());
        Assert.False(request.TryGetProperty("from", out _));
    }

    [Fact]
    public async Task SearchAfter_TraversesFullPartialAndEmptyPagesWithoutLosingDocuments()
    {
        var scenario = new Scenario(
            new Step("_search", SearchResponse(3, 11, 22)),
            new Step("_count", CountResponse(3)),
            new Step("_search", SearchResponse(3, 33)),
            new Step("_count", CountResponse(3)),
            new Step("_search", SearchResponse(3)),
            new Step("_count", CountResponse(3)));
        var ids = new List<string?>();
        long? cursor = null;
        var expectedCursors = new long?[] { 22, 33, null };

        foreach (var expected in expectedCursors)
        {
            var (documents, result) = await scenario.Service.SearchAfter(Parameters(), 2, cursor);
            ids.AddRange(documents.Select(document => document.Id));
            Assert.Equal(expected, result.SearchAfter);
            Assert.Equal(2, result.PageSize);
            Assert.Equal(3L, result.TotalResults);
            cursor = result.SearchAfter;
        }

        Assert.Equal(new[] { "publication-11", "publication-22", "publication-33" }, ids);
        Assert.Equal(ids.Count, ids.Distinct().Count());
        var requests = scenario.AssertRequests("_search", "_count", "_search", "_count", "_search", "_count");
        Assert.False(requests[0].TryGetProperty("search_after", out _));
        Assert.Equal(22, requests[2].GetProperty("search_after")[0].GetInt64());
        Assert.Equal(33, requests[4].GetProperty("search_after")[0].GetInt64());
        for (var index = 0; index < requests.Length; index += 2)
        {
            AssertExportRequest(requests[index], 2);
            AssertQueryParity(requests[index], requests[index + 1]);
            Assert.False(requests[index + 1].TryGetProperty("size", out _));
            AssertNameFilter(requests[index]);
        }
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(9007199254740993L)]
    public async Task SearchAfter_PreservesLongCursorAndUsesLastHitSortNotSourceId(long lastSort)
    {
        var scenario = new Scenario(
            new Step("_search", SearchResponse(10000, 5, lastSort)),
            new Step("_count", CountResponse(25001)));

        var (documents, result) = await scenario.Service.SearchAfter(Parameters(), 2, 0);

        Assert.Equal(2, documents.Count());
        Assert.Equal(lastSort, result.SearchAfter);
        Assert.Equal(25001L, result.TotalResults);
        var requests = scenario.AssertRequests("_search", "_count");
        Assert.Equal(0, requests[0].GetProperty("search_after")[0].GetInt64());
        AssertQueryParity(requests[0], requests[1]);
    }

    [Fact]
    public async Task SearchAfter_EmptyFirstPageStillCountsAndHasNoNextCursor()
    {
        var scenario = new Scenario(
            new Step("_search", SearchResponse(0)),
            new Step("_count", CountResponse(0)));

        var (documents, result) = await scenario.Service.SearchAfter(Parameters(), 7, null);

        Assert.Empty(documents);
        Assert.Null(result.SearchAfter);
        Assert.Equal(0L, result.TotalResults);
        Assert.Equal(7, result.PageSize);
        scenario.AssertRequests("_search", "_count");
    }

    [Theory]
    [InlineData(Operation.Search, 400)]
    [InlineData(Operation.Single, 400)]
    [InlineData(Operation.Export, 400)]
    [InlineData(Operation.Search, 503)]
    [InlineData(Operation.Single, 503)]
    [InlineData(Operation.Export, 503)]
    public async Task SearchFailure_CurrentlyLooksLikeEmptySuccessWithDefaultClientSettings(Operation operation, int status)
    {
        // P4-SEARCH: characterization only; an invalid response is not a successful empty search.
        var scenario = new Scenario(operation == Operation.Export
            ? new[] { new Step("_search", ErrorResponse(status), status), new Step("_count", CountResponse(17)) }
            : new[] { new Step("_search", ErrorResponse(status), status) });

        switch (operation)
        {
            case Operation.Search:
                var (documents, result) = await scenario.Service.Search(Parameters(), 1, 10);
                Assert.Empty(documents);
                Assert.Equal(0, result.TotalResults);
                break;
            case Operation.Single:
                Assert.Null(await scenario.Service.GetSingle("synthetic-id"));
                break;
            case Operation.Export:
                var (exported, export) = await scenario.Service.SearchAfter(Parameters(), 10, null);
                Assert.Empty(exported);
                Assert.Null(export.SearchAfter);
                Assert.Equal(17L, export.TotalResults);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operation));
        }
        scenario.AssertRequests(operation == Operation.Export ? new[] { "_search", "_count" } : new[] { "_search" });
        Assert.False(scenario.Calls.Single(call => call.Uri.AbsolutePath.EndsWith("/_search", StringComparison.Ordinal))
            .Success);
    }

    [Theory]
    [InlineData(Operation.Search, 400)]
    [InlineData(Operation.Single, 400)]
    [InlineData(Operation.Export, 400)]
    [InlineData(Operation.Search, 503)]
    [InlineData(Operation.Single, 503)]
    [InlineData(Operation.Export, 503)]
    public async Task SearchFailure_PropagatesWhenClientIsConfiguredToThrow(Operation operation, int status)
    {
        var scenario = new Scenario(true, new Step("_search", ErrorResponse(status), status));

        await Assert.ThrowsAsync<ElasticsearchClientException>(async () =>
        {
            switch (operation)
            {
                case Operation.Search:
                    await scenario.Service.Search(Parameters(), 1, 10);
                    break;
                case Operation.Single:
                    await scenario.Service.GetSingle("synthetic-id");
                    break;
                case Operation.Export:
                    await scenario.Service.SearchAfter(Parameters(), 10, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation));
            }
        });

        scenario.AssertRequests("_search");
    }

    [Theory]
    [InlineData(false, 400)]
    [InlineData(true, 400)]
    [InlineData(false, 503)]
    [InlineData(true, 503)]
    public async Task CountFailure_CurrentlyReturnsZeroOrUnknownTotalInsteadOfSurfacingFailure(bool throwExceptions, int status)
    {
        // P4-COUNT: both invalid count responses and swallowed exceptions need separate review.
        var scenario = new Scenario(throwExceptions,
            new Step("_search", SearchResponse(1, 42)),
            new Step("_count", ErrorResponse(status), status));

        var (documents, result) = await scenario.Service.SearchAfter(Parameters(), 10, null);

        Assert.Equal("publication-42", Assert.Single(documents).Id);
        Assert.Equal(42L, result.SearchAfter);
        Assert.Equal(throwExceptions ? (long?)null : 0L, result.TotalResults);
        scenario.AssertRequests("_search", "_count");
        Assert.False(scenario.Calls.Single(call => call.Uri.AbsolutePath.EndsWith("/_count", StringComparison.Ordinal))
            .Success);
    }

    private static PublicationSearchParameters Parameters() => new()
    {
        Name = "Synthetic publication",
        TypeCode = "A1",
        ShowOrganisationPartofCoPublication = true
    };

    private static void AssertNameFilter(JsonElement request)
    {
        var query = request.GetProperty("query").GetProperty("bool");
        Assert.Equal("Synthetic publication", query.GetProperty("must")[0]
            .GetProperty("match_phrase").GetProperty("Name").GetProperty("query").GetString());
        Assert.Equal("A1", query.GetProperty("filter")[0].GetProperty("term")
            .GetProperty("Type.Code").GetProperty("value").GetString());
    }

    private static void AssertExportRequest(JsonElement request, int pageSize)
    {
        Assert.Equal(pageSize, request.GetProperty("size").GetInt32());
        Assert.False(request.TryGetProperty("from", out _));
        Assert.Equal("asc", request.GetProperty("sort")[0].GetProperty("ExportSortId").GetProperty("order").GetString());
        Assert.Single(request.GetProperty("sort").EnumerateArray());
    }

    private static void AssertQueryParity(JsonElement search, JsonElement count) =>
        Assert.True(JsonElement.DeepEquals(search.GetProperty("query"), count.GetProperty("query")));

    private static string SearchResponse(long total, params long[] sorts) => JsonSerializer.Serialize(new
    {
        took = 0,
        timed_out = false,
        _shards = new { total = 1, successful = 1, skipped = 0, failed = 0 },
        hits = new
        {
            total = new { value = total, relation = "eq" },
            max_score = (double?)null,
            hits = sorts.Select((sort, index) => new
            {
                _index = "synthetic-publications",
                _id = $"document-{index}",
                _score = (double?)null,
                _source = new { Id = $"publication-{sort}", Name = "Synthetic publication", ExportSortId = -1 },
                sort = new[] { sort }
            }).ToArray()
        }
    });

    private static string CountResponse(long count) => JsonSerializer.Serialize(new
    {
        count,
        _shards = new { total = 1, successful = 1, skipped = 0, failed = 0 }
    });

    private static string ErrorResponse(int status) => JsonSerializer.Serialize(new
    {
        error = new
        {
            root_cause = new[] { new { type = "synthetic_failure", reason = "Synthetic query failure" } },
            type = "synthetic_failure",
            reason = "Synthetic query failure"
        },
        status
    });

    public enum Operation { Search, Single, Export }

    private sealed record Step(string Endpoint, string Body, int Status = 200);

    private sealed class Scenario
    {
        private readonly ScriptedConnection _connection;
        private readonly Step[] _steps;
        public List<IApiCallDetails> Calls { get; } = new();
        public ElasticSearchService<PublicationSearchParameters, Publication> Service { get; }

        public Scenario(params Step[] steps) : this(false, steps) { }

        public Scenario(bool throwExceptions, params Step[] steps)
        {
            _steps = steps;
            _connection = new ScriptedConnection(steps);
            var settings = new ConnectionSettings(
                    new SingleNodeConnectionPool(new Uri("http://elasticsearch.test:9200")), _connection)
                .DefaultFieldNameInferrer(name => name)
                .MaximumRetries(0)
                .DisableDirectStreaming()
                .OnRequestCompleted(Calls.Add);
            if (throwExceptions)
                settings.ThrowExceptions();
            var client = new ElasticClient(settings);
            var indices = new IndexNameSettings { [typeof(Publication).FullName!] = "synthetic-publications" };
            Service = new ElasticSearchService<PublicationSearchParameters, Publication>(
                client, new PublicationQueryGenerator(indices));
        }

        public JsonElement[] AssertRequests(params string[] endpoints)
        {
            Assert.Equal(0, _connection.RemainingSteps);
            var calls = Calls.Where(call => call.Uri.AbsolutePath != "/").ToArray();
            Assert.Equal(endpoints.Select(endpoint => $"/synthetic-publications/{endpoint}"),
                calls.Select(call => call.Uri.AbsolutePath));
            Assert.All(calls, call => Assert.Equal(Elasticsearch.Net.HttpMethod.POST, call.HttpMethod));
            Assert.Equal(_steps.Length, calls.Length);
            for (var index = 0; index < calls.Length; index++)
            {
                Assert.Equal(_steps[index].Status, calls[index].HttpStatusCode);
                Assert.Equal(_steps[index].Status == 200, calls[index].Success);
                if (_steps[index].Status == 200)
                    Assert.Null(calls[index].OriginalException);
            }
            return calls.Select(call =>
            {
                Assert.NotNull(call.RequestBodyInBytes);
                using var document = JsonDocument.Parse(call.RequestBodyInBytes!);
                return document.RootElement.Clone();
            }).ToArray();
        }
    }

    private sealed class ScriptedConnection(IEnumerable<Step> steps) : MockElasticConnection
    {
        private readonly Queue<Step> _steps = new(steps);
        public int RemainingSteps => _steps.Count;

        public override TResponse Request<TResponse>(RequestData requestData)
        {
            if (requestData.Method == Elasticsearch.Net.HttpMethod.GET && requestData.Uri.AbsolutePath == "/")
                return base.Request<TResponse>(requestData);
            var step = Next(requestData);
            return ReturnConnectionStatus<TResponse>(
                requestData, Encoding.UTF8.GetBytes(step.Body), step.Status, RequestData.DefaultJsonMimeType);
        }

        public override Task<TResponse> RequestAsync<TResponse>(RequestData requestData, CancellationToken cancellationToken)
        {
            if (requestData.Method == Elasticsearch.Net.HttpMethod.GET && requestData.Uri.AbsolutePath == "/")
                return base.RequestAsync<TResponse>(requestData, cancellationToken);
            var step = Next(requestData);
            return ReturnConnectionStatusAsync<TResponse>(
                requestData, cancellationToken, Encoding.UTF8.GetBytes(step.Body), step.Status, RequestData.DefaultJsonMimeType);
        }

        private Step Next(RequestData request)
        {
            Assert.True(_steps.Count > 0, $"Unexpected Elasticsearch request: {request.Method} {request.Uri}");
            var step = _steps.Dequeue();
            Assert.Equal(Elasticsearch.Net.HttpMethod.POST, request.Method);
            Assert.Equal($"/synthetic-publications/{step.Endpoint}", request.Uri.AbsolutePath);
            return step;
        }
    }
}
