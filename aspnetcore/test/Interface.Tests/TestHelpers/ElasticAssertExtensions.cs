using Elasticsearch.Net;
using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC.PublicApi.Tests.TestHelpers;

public static class ElasticAssertExtensions
{
    public static void AssertCreateIndexCalled(this List<IApiCallDetails> apiCalls, string indexName)
    {
        apiCalls
            .Should()
            .Contain(apiRequest =>
                    apiRequest.HttpMethod == HttpMethod.PUT &&
                    apiRequest.Uri.AbsolutePath == $"/{indexName}",
                $"Index '{indexName}' should have been created");
    }

    public static void AssertBulkAliasCalled(this List<IApiCallDetails> apiCalls, string aliasName, string createdIndex, string deletedIndex)
    {
        var aliasCalls = apiCalls.Where(apiRequest =>
            apiRequest.HttpMethod == HttpMethod.POST &&
            apiRequest.Uri.AbsolutePath == $"/_aliases");

        var aliasRequests = aliasCalls
            .Where(c => c.RequestBodyInBytes != null)
            .Select(c => ConvertBytes(c.RequestBodyInBytes))
            .SelectMany(rootObject => rootObject.actions);

        aliasRequests
            .Should()
            .Contain(action => action.add != null && action.add.index == createdIndex && action.add.alias == aliasName)
            .And.Contain(action => action.remove != null && action.remove.index == deletedIndex && action.remove.alias == aliasName);
    }

    private static BulkAliasRequestMock ConvertBytes(byte[] bytes)
    {
        var jsonString = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<BulkAliasRequestMock>(jsonString);
    }


}

public class BulkAliasRequestMock
{
    public BulkAction[]? actions { get; set; }
}

public class BulkAction
{
    public Remove? remove { get; set; }
    public Add? add { get; set; }
}

public class Remove
{
    public string? alias { get; set; }
    public string? index { get; set; }
}

public class Add
{
    public string alias { get; set; }
    public string index { get; set; }
}