using Api.DatabaseContext;
using Api.Models.Entities;
using Api.Test.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;
using FundingCall = Api.Models.FundingCall.FundingCall;

namespace Api.Test
{
    public class FundingCallSystemTest : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> _factory;
        private readonly ApiDbContext _dbContext;
        private readonly HttpClient _client;
        private readonly string _apiBaseUrl = "v1/FundingCall";

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
            var apiUrl = $"{_apiBaseUrl}?{urlParams}";

            // Act
            var response = await _client.GetAsync(apiUrl);

            // Assert
            var fundingCalls = await response.GetResponseObject<IEnumerable<FundingCall>>();

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
            var apiUrl = $"{_apiBaseUrl}?{urlParams}";

            // Act
            var response = await _client.GetAsync(apiUrl);

            // Assert
            var fundingCalls = await response.GetResponseObject<IEnumerable<FundingCall>>();

            fundingCalls
                .Should()
                .BeEmpty();
        }

        /// <summary>
        /// Test for running funding call query against a real db and elasticsearch instance.
        /// </summary>
        [Fact(Skip = "Run manually when needed.")]
        public async void POST_ShouldAddFundingCallToDb()
        {
            // Arrange
            var apiUrl = $"{_apiBaseUrl}";
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
                [Name("apurahahaku")] = fc =>
                    NamesShouldMatch(fc, "apurahahaku"),

                // should find only calls with the given name
                [Name("apurahojen")] = fc =>
                    NamesShouldMatch(fc, "apurahojen"),

                // should find only calls with the given foundation name
                [FoundationName("säätiö")] = fc =>
                    FoundationNamesShouldMatch(fc, "säätiö"),

                // should find calls with the given foundation business id
                [FoundationBusinessId("02509")] = fc =>
                    FoundationBusinessIdShouldEqual(fc, "02509"),

                // should find calls with the given category code
                [Category("18698")] = fc =>
                    CategoriesShouldContain(fc, "18698"),

                // should find calls which are open later than the given date 
                [DateFrom("2019-02-01")] = fc =>
                    DatesShouldBeBetween(fc, new DateTime(2019, 2, 1), DateTime.MaxValue),

                // should find calls which are closed before than the given date 
                [DateTo("2014-02-01")] = fc =>
                    DatesShouldBeBetween(fc, DateTime.MinValue, new DateTime(2014, 2, 1)),

                // should find calls open between given dates
                [DateFrom("2013-02-01") + "&" + DateTo("2013-03-01")] = fc =>
                    DatesShouldBeBetween(fc, new DateTime(2013, 2, 1), new DateTime(2013, 3, 1)),

                // should find calls with the given name which are open on given dates
                [Name("tiekartalla") + "&" + DateFrom("2019-02-01") + "&" + DateTo("2020-02-01")] = fc =>
                    NamesShouldMatch(fc, "tiekartalla") &&
                    DatesShouldBeBetween(fc, new DateTime(2019, 2, 1), new DateTime(2020, 2, 1))

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
                "foundationBusinessId=0809",
                // should not find calls with open dates starting in the future.
                Name("apurahahaku") + "&" + DateFrom("2040-01-01"),
                // should not find calls with the given name on the given date range
                Name("tiekartalla") + "&" + DateFrom("2019-06-01") + "&" + DateTo("2022-12-31")
            };

            foreach (var testCase in testCasesWhichShouldNotFindAnything)
            {
                yield return new object[] { testCase };
            }
        }

        private static string Name(string name)
        {
            return $"name={name}";
        }

        private static string FoundationName(string name)
        {
            return $"foundationName={name}";
        }

        private static string FoundationBusinessId(string id)
        {
            return $"foundationBusinessId={id}";
        }

        private static string DateFrom(string date)
        {
            return $"dateFrom={date}";
        }

        private static string DateTo(string date)
        {
            return $"dateTo={date}";
        }

        private static string Category(string categoryCode)
        {
            return $"categoryCode={categoryCode}";
        }

        private static bool NamesShouldMatch(FundingCall fc, string text)
        {
            return fc != null &&
                ((fc.NameFi != null && fc.NameFi.Contains(text, StringComparison.InvariantCultureIgnoreCase)) ||
                (fc.NameSv != null && fc.NameSv.Contains(text, StringComparison.InvariantCultureIgnoreCase)) ||
                (fc.NameEn != null && fc.NameEn.Contains(text, StringComparison.InvariantCultureIgnoreCase)));
        }

        private static bool FoundationNamesShouldMatch(FundingCall fc, string text)
        {
            return fc.Foundation != null &&
                fc.Foundation.Any(f =>
                    (f.FoundationNameFi != null && f.FoundationNameFi.Contains(text, StringComparison.InvariantCultureIgnoreCase)) ||
                    (f.FoundationNameSv != null && f.FoundationNameSv.Contains(text, StringComparison.InvariantCultureIgnoreCase)) ||
                    (f.FoundationNameEn != null && f.FoundationNameEn.Contains(text, StringComparison.InvariantCultureIgnoreCase)));
        }

        private static bool FoundationBusinessIdShouldEqual(FundingCall fc, string text)
        {
            return fc.Foundation != null && fc.Foundation.Any(f => f.FoundationBusinessId == text);
        }

        private static bool CategoriesShouldContain(FundingCall fc, string code)
        {
            return fc.Categories != null &&
                                fc.Categories.Any(c => c.CategoryCodeValue == code);
        }

        private static bool DatesShouldBeBetween(FundingCall fc, DateTime start, DateTime end)
        {
            return (fc.CallProgrammeOpenDate == null || fc.CallProgrammeOpenDate <= end) &&
                    (fc.CallProgrammeDueDate == null || fc.CallProgrammeDueDate >= start);
        }

    }
}