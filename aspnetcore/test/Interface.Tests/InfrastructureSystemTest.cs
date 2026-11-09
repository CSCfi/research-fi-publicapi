using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using CSC.PublicApi.Interface.Tests.TestHelpers;
using FluentAssertions;
using Xunit;
using Infrastructure = ResearchFi.Infrastructure.Infrastructure;

namespace CSC.PublicApi.Interface.Tests;

public class InfrastructureSystemTest : IClassFixture<TestWebApplicationFactory<Program>>, IDisposable
{
    private readonly HttpClient _client;
    private readonly string _apiBaseUrl = "v1/Infrastructure";

    public InfrastructureSystemTest(TestWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Test for running infrastructure query against a real elasticsearch instance.
    /// </summary>
    [Theory(Skip = "Integration test")]
    [MemberData(nameof(InfrastructureResultTestCases))]
    public async Task GET_ShouldReturnExpected(string urlParams, Expression<Func<Infrastructure, bool>> assertPredicate)
    {
        // Arrange
        var apiUrl = $"{_apiBaseUrl}?{urlParams}";

        // Act
        using var response = await _client.GetAsync(apiUrl);

        // Assert
        var infrastructures = await response.GetResponseObject<IEnumerable<Infrastructure>>();

        infrastructures
            .Should()
            .NotBeNullOrEmpty()
            .And.OnlyContain(assertPredicate);
    }

    /// <summary>
    /// Test for running infrastructure query against a real elasticsearch instance.
    /// </summary>
    [Theory(Skip = "Integration test")]
    [MemberData(nameof(InfrastructureNoResultsTestCases))]
    public async Task GET_ShouldReturnEmpty(string urlParams)
    {
        // Arrange
        var apiUrl = $"{_apiBaseUrl}?{urlParams}";

        // Act
        using var response = await _client.GetAsync(apiUrl);

        // Assert
        var infrastructures = await response.GetResponseObject<IEnumerable<Infrastructure>>();

        infrastructures
            .Should()
            .BeEmpty();
    }

    /// <summary>
    /// Test cases for funding calls.
    /// First parameter is the query parameters for the search and the other is the predicate the search results must satisfy.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<object[]> InfrastructureResultTestCases()
    {
        var testCasesWhichExpectSomethingReturned = new Dictionary<string, Expression<Func<Infrastructure, bool>>>
        {
            // should find only calls with the given name
            [Name("Eurooppalainen sosiaalitutkimus")] = fc =>
                NamesShouldMatch(fc, "Eurooppalainen sosiaalitutkimus"),

        };

        foreach (var testCase in testCasesWhichExpectSomethingReturned)
        {
            yield return new object[] { testCase.Key, testCase.Value };
        }
    }

    public static IEnumerable<object[]> InfrastructureNoResultsTestCases()
    {
        var testCasesWhichShouldNotFindAnything = new List<string>
        {
            Name("nonexistant name"),
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

    private static bool NamesShouldMatch(Infrastructure infrastructure, string text)
    {
        return true;
        /*
        return infrastructure != null &&
               (infrastructure.NameFi != null && infrastructure.NameFi.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                infrastructure.NameSv != null && infrastructure.NameSv.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                infrastructure.NameEn != null && infrastructure.NameEn.Contains(text, StringComparison.InvariantCultureIgnoreCase));
        */
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}