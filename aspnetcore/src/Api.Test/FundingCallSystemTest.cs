using Api.DatabaseContext;
using Api.Models.Entities;
using Api.Models.FundingCall;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test
{
    public class FundingCallSystemTest : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> _factory;
        private readonly ApiDbContext _dbContext;
        private readonly HttpClient _client;

        public FundingCallSystemTest(TestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
            _client = _factory.CreateClient();
        }

        /// <summary>
        /// Test for running funding call query against a real elasticsearch instance.
        /// </summary>
        [Theory]
        [MemberData(nameof(FundingCallResultTestCases))]
        public async void GET_ShouldReturnExpected(string urlParams, Expression<Func<FundingCall, bool>> assertPredicate)
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
        public async void GET_ShouldReturnEmpty(string urlParams)
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
        /// Test for running funding call query against a real db and elasticsearch instance.
        /// </summary>
        [Fact]
        public async void POST_ShouldAddFundingCallToDb()
        {
            // Arrange
            var apiUrl = $"FundingCall";
            var fundingCallToAdd = new FundingCall
            {
                NameFi = $"test funding call {Guid.NewGuid()}"
            };
            var content = JsonContent.Create(fundingCallToAdd);

            // Act
            var response = await _client.PostAsync(apiUrl, content);

            // Assert
            response.EnsureSuccessStatusCode();
            var addedFundingCall = _dbContext
                .Set<DimCallProgramme>()
                .SingleOrDefault(x => x.NameFi == fundingCallToAdd.NameFi);

            addedFundingCall.Should().NotBeNull();
            
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