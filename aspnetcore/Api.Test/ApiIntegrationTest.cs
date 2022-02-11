using Api.Models.FundingCall;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test
{
    public class ApiIntegrationTest : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ApiIntegrationTest(TestWebApplicationFactory<Program> factory)
        {
            _factory = factory;

            _client = _factory.CreateClient();
        }

        /// <summary>
        /// Test for running search against a real elasticsearch instance.
        /// Meant for manual testing.
        /// </summary>
        [Fact(Skip ="Only for manual testing")]
        public async void FundingCall_ManualSystemTest()
        {
            // Arrange
            var apiUrl = "FundingCall?name=apuraha";

            // Act
            var response = await _client.GetAsync(apiUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var fundingCalls = await GetResponseObject<IEnumerable<FundingCall>>(response);
            fundingCalls.First().NameFi.Should().NotBeNullOrWhiteSpace();

        }

        private static async Task<T> GetResponseObject<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            
            if(string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidOperationException("Can not parse empty json.");
            }


            var deserializedContents = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if(deserializedContents == null)
            {
                throw new InvalidOperationException($"Failed to parse json: '{json}'.");
            }

            return deserializedContents;
        }
    }
}