using Api.Models.FundingCall;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        /// Test for running funding call query against a real elasticsearch instance.
        /// </summary>
        [Theory]
        [MemberData(nameof(FundingCallResultTestCases))]
        public async void FundingCall_ShouldReturnExpected(string urlParams, Expression<Func<FundingCall, bool>> assertPredicate)
        {
            // Arrange
            var apiUrl = $"FundingCall?{urlParams}";

            // Act
            var response = await _client.GetAsync(apiUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var fundingCalls = await GetResponseObject<IEnumerable<FundingCall>>(response);

            fundingCalls
                .Should()
                .NotBeNullOrEmpty()
                .And.OnlyContain(assertPredicate);
        }

        /// <summary>
        /// Test for running funding call query against a real elasticsearch instance.
        /// </summary>
        [Theory]
        [MemberData(nameof(FundingCallNoResultsTestCases))]
        public async void FundingCall_ShouldReturnEmpty(string urlParams)
        {
            // Arrange
            var apiUrl = $"FundingCall?{urlParams}";

            // Act
            var response = await _client.GetAsync(apiUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var fundingCalls = await GetResponseObject<IEnumerable<FundingCall>>(response);

            fundingCalls
                .Should()
                .BeEmpty();
        }

        /// <summary>
        /// Test cases for funding calls.
        /// First parameter is the query parameters for the search and the other is the predicate the search results must satisfy.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> FundingCallResultTestCases()
        {
            var testCasesWhichExpectSomethingReturned = new Dictionary<string, Expression<Func<FundingCall, bool>>>
            {
                // should find only calls with the given name
                ["name=apurahahaku"] = fc =>
                                fc.NameFi.Contains("apurahahaku", StringComparison.InvariantCultureIgnoreCase) ||
                                fc.NameSv.Contains("apurahahaku", StringComparison.InvariantCultureIgnoreCase) ||
                                fc.NameEn.Contains("apurahahaku", StringComparison.InvariantCultureIgnoreCase),
                // should find only calls with the given foundation name
                ["foundationName=säätiö"] = fc =>
                                fc.Foundation.Any(f =>
                                    f.FoundationNameFi.Contains("säätiö", StringComparison.InvariantCultureIgnoreCase) ||
                                    f.FoundationNameSv.Contains("säätiö", StringComparison.InvariantCultureIgnoreCase) ||
                                    f.FoundationNameEn.Contains("säätiö", StringComparison.InvariantCultureIgnoreCase)),
                // should find calls with the given foundation business id
                ["foundationBusinessId=0809036-2"] = fc => fc.Foundation.Any(f =>
                                    f.FoundationBusinessId == "0809036-2")
            };

            foreach (var testCase in testCasesWhichExpectSomethingReturned)
            {
                yield return new object[] { testCase.Key, testCase.Value };
            }


        }

        public static IEnumerable<object[]> FundingCallNoResultsTestCases()
        {
            var testCasesWhichShouldNotFindAnything = new List<string>
            {
                // should not find calls with partial match of the business id
                "foundationBusinessId=0809"
            };

            foreach (var testCase in testCasesWhichShouldNotFindAnything)
            {
                yield return new object[] { testCase };
            }
        }

        private static async Task<T> GetResponseObject<T>(HttpResponseMessage response)
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