using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Newtonsoft.Json;

namespace CSC.PublicApi.Interface.Tests.TestHelpers;

public class MockElasticConnection : InMemoryConnection
{
    private readonly List<string> _mockIndices;

    public MockElasticConnection(List<string>? indices = null)
    {
        _mockIndices = indices ?? new();
    }

    private (byte[]?, int, string) DetermineResponse(RequestData requestData)
    {
        int statusCode = 200;
        string contentType = RequestData.DefaultJsonMimeType;
        byte[]? responseBody;

        if (requestData.Method == HttpMethod.DELETE && IsIndexPath(requestData.Uri))
        {
            _mockIndices.Remove(requestData.Uri.AbsolutePath);
            responseBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { acknowledged = true }));
        }
        else if (requestData.Method == HttpMethod.PUT && IsIndexPath(requestData.Uri))
        {
            _mockIndices.Add(requestData.Uri.AbsolutePath);
            responseBody = JsonResponse(new
            {
                acknowledged = true,
                shards_acknowledged = true,
                index = requestData.Uri.AbsolutePath.Trim('/')
            });
        }
        else if (requestData.Method == HttpMethod.HEAD && IsIndexPath(requestData.Uri))
        {
            statusCode = _mockIndices.Contains(requestData.Uri.AbsolutePath) ? 200 : 404;
            responseBody = Array.Empty<byte>();
        }
        else if (requestData.Method == HttpMethod.POST && requestData.Uri.AbsolutePath == "/_aliases")
        {
            responseBody = JsonResponse(new { acknowledged = true });
        }
        else if (requestData.Method == HttpMethod.POST && requestData.Uri.AbsolutePath.EndsWith("/_bulk"))
        {
            responseBody = CreateBulkResponse(requestData);
        }
        else if (requestData.Method == HttpMethod.GET && requestData.Uri.AbsolutePath.StartsWith("/_cluster/health/"))
        {
            responseBody = JsonResponse(new
            {
                cluster_name = "test",
                status = "yellow",
                timed_out = false,
                number_of_nodes = 1,
                number_of_data_nodes = 1,
                active_primary_shards = 1,
                active_shards = 1,
                relocating_shards = 0,
                initializing_shards = 0,
                unassigned_shards = 0,
                delayed_unassigned_shards = 0,
                number_of_pending_tasks = 0,
                number_of_in_flight_fetch = 0,
                task_max_waiting_in_queue_millis = 0,
                active_shards_percent_as_number = 100.0
            });
        }
        else if (requestData.Method == HttpMethod.GET && requestData.Uri.AbsolutePath == "/")
        {
            responseBody = JsonResponse(new
            {
                name = "test-node",
                cluster_name = "test",
                cluster_uuid = "synthetic-cluster-id",
                version = new
                {
                    number = "7.17.0",
                    build_flavor = "default",
                    build_type = "tar",
                    build_hash = "synthetic",
                    build_date = "2024-01-01T00:00:00.000Z",
                    build_snapshot = false,
                    lucene_version = "8.11.1",
                    minimum_wire_compatibility_version = "6.8.0",
                    minimum_index_compatibility_version = "6.0.0"
                },
                tagline = "You Know, for Search"
            });
        }
        else if (requestData.Method == HttpMethod.POST && requestData.Uri.AbsolutePath.EndsWith("/_search"))
        {
            responseBody = JsonResponse(new
            {
                took = 0,
                timed_out = false,
                _shards = new { total = 1, successful = 1, skipped = 0, failed = 0 },
                hits = new
                {
                    total = new { value = 0, relation = "eq" },
                    max_score = (double?)null,
                    hits = Array.Empty<object>()
                }
            });
        }
        else
        {
            throw new InvalidOperationException(
                $"Unexpected Elasticsearch request: {requestData.Method} {requestData.Uri}");
        }

        return (responseBody, statusCode, contentType);
    }

    private static byte[] JsonResponse(object response)
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
    }

    private static bool IsIndexPath(Uri uri)
    {
        var indexName = uri.AbsolutePath.Trim('/');
        return indexName.Length > 0 && !indexName.StartsWith('_') && !indexName.Contains('/');
    }

    private static byte[] CreateBulkResponse(RequestData requestData)
    {
        var indexName = requestData.Uri.AbsolutePath.Trim('/').Replace("/_bulk", string.Empty);
        var items = new[]
        {
            new Dictionary<string, object>
            {
                ["index"] = new
                {
                    _index = indexName,
                    _type = "_doc",
                    _id = "test-document",
                    _version = 1,
                    status = 201,
                    result = "created",
                    _seq_no = 1,
                    _primary_term = 1,
                    _shards = new { total = 1, successful = 1, failed = 0 }
                }
            }
        };

        return JsonResponse(new { took = 0, errors = false, items });
    }

    public override TResponse Request<TResponse>(RequestData requestData)
    {
        var (responseBody, statusCode, contentType) = DetermineResponse(requestData);
        return ReturnConnectionStatus<TResponse>(requestData, responseBody, statusCode, contentType);
    }

    public override Task<TResponse> RequestAsync<TResponse>(RequestData requestData, CancellationToken cancellationToken)
    {
        var (responseBody, statusCode, contentType) = DetermineResponse(requestData);
        return ReturnConnectionStatusAsync<TResponse>(requestData, cancellationToken, responseBody, statusCode, contentType);
    }
}