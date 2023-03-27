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

        if (requestData.Method == HttpMethod.DELETE)
        {
            _mockIndices.Remove(requestData.Uri.AbsolutePath);
            responseBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { acknowledged = true }));
        }
        else if (requestData.Method == HttpMethod.PUT)
        {
            _mockIndices.Add(requestData.Uri.AbsolutePath);
            responseBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { acknowledged = true }));
        }
        else if (requestData.Method == HttpMethod.HEAD)
        {
            statusCode = _mockIndices.Contains(requestData.Uri.AbsolutePath) ? 200 : 404;
            responseBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { }));
        }
        else
        {
            responseBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { }));
        }

        return (responseBody, statusCode, contentType);
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