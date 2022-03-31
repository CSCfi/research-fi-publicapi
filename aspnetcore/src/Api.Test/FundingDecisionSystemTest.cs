using Api.DatabaseContext;
using Api.Models.FundingDecision;
using Api.Test.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using Xunit;

namespace Api.Test
{
    public class FundingDecisionSystemTest : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> _factory;
        private readonly ApiDbContext _dbContext;
        private readonly HttpClient _client;
        private readonly string _apiBaseUrl = "v1/FundingDecision";

        public FundingDecisionSystemTest(TestWebApplicationFactory<Program> factory)
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
        [MemberData(nameof(FundingDecisionResultTestCases))]
        public async void GET_ShouldReturnExpected(string urlParams, Expression<Func<FundingDecision, bool>> assertPredicate)
        {
            // Arrange
            var apiUrl = $"{_apiBaseUrl}?{urlParams}";

            // Act
            var response = await _client.GetAsync(apiUrl);

            // Assert
            var fundingCalls = await response.GetResponseObject<IEnumerable<FundingDecision>>();

            fundingCalls
                .Should()
                .NotBeNullOrEmpty()
                .And.OnlyContain(assertPredicate);
        }

        /// <summary>
        /// Test for running funding call query against a real elasticsearch instance.
        /// </summary>
        [Theory]
        [MemberData(nameof(FundingDecisionNoResultsTestCases))]
        public async void GET_ShouldReturnEmpty(string urlParams)
        {
            // Arrange
            var apiUrl = $"{_apiBaseUrl}?{urlParams}";

            // Act
            var response = await _client.GetAsync(apiUrl);

            // Assert
            var fundingCalls = await response.GetResponseObject<IEnumerable<FundingDecision>>();

            fundingCalls
                .Should()
                .BeEmpty();
        }

        /// <summary>
        /// Test cases for funding decisions.
        /// First parameter is the query parameters for the search and the other is the predicate the search results must satisfy.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> FundingDecisionResultTestCases()
        {
            var testCasesWhichExpectSomethingReturned = new Dictionary<string, Expression<Func<FundingDecision, bool>>>
            {
                // should find only calls with the given name
                ["name=Architectures"] = fd => fd != null &&
                                ((fd.NameFi != null && fd.NameFi.Contains("Architectures", StringComparison.InvariantCultureIgnoreCase)) ||
                                (fd.NameSv != null && fd.NameSv.Contains("Architectures", StringComparison.InvariantCultureIgnoreCase)) ||
                                (fd.NameEn != null && fd.NameEn.Contains("Architectures", StringComparison.InvariantCultureIgnoreCase))),
            };

            foreach (var testCase in testCasesWhichExpectSomethingReturned)
            {
                yield return new object[] { testCase.Key, testCase.Value };
            }


        }

        public static IEnumerable<object[]> FundingDecisionNoResultsTestCases()
        {
            var testCasesWhichShouldNotFindAnything = new List<string>
            {
                // should not find calls with non-existant name
                "name=somenotexistingdecisionname"
            };

            foreach (var testCase in testCasesWhichShouldNotFindAnything)
            {
                yield return new object[] { testCase };
            }
        }

    }
}