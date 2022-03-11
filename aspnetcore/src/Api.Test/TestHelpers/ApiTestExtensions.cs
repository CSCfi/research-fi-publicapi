using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Test.TestHelpers
{
    public static class ApiTestExtensions
    {
        public static async Task<T> GetResponseObject<T>(this HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidOperationException("Can not parse empty json.");
            }


            var deserializedContents = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (deserializedContents == null)
            {
                throw new InvalidOperationException($"Failed to parse json: '{json}'.");
            }

            return deserializedContents;
        }
    }
}
