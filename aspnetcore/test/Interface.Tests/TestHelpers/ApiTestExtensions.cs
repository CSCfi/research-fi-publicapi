using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSC.PublicApi.Interface.Tests.TestHelpers;

public static class ApiTestExtensions
{
    public static async Task<T> GetResponseObject<T>(this HttpResponseMessage response)
    {
        var responseContentString = await response.Content.ReadAsStringAsync();

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Request failed. Response is '{responseContentString}'.", ex);
        }

        if (string.IsNullOrWhiteSpace(responseContentString))
        {
            throw new InvalidOperationException("Can not parse empty json.");
        }


        var deserializedContents = JsonSerializer.Deserialize<T>(responseContentString, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        if (deserializedContents == null)
        {
            throw new InvalidOperationException($"Failed to parse json: '{responseContentString}'.");
        }

        return deserializedContents;
    }
}