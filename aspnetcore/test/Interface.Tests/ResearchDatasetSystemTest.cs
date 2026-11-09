using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using ResearchFi.ResearchDataset.V1;
using CSC.PublicApi.Interface.Tests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace CSC.PublicApi.Interface.Tests;

public class ResearchDatasetSystemTest : IClassFixture<TestWebApplicationFactory<Program>>, IDisposable
{
    private readonly HttpClient _client;
    private readonly string _apiBaseUrl = "v1/ResearchDataset";

    public ResearchDatasetSystemTest(TestWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Test for running ResearchDataset query against a real elasticsearch instance.
    /// </summary>
    [Theory(Skip = "Integration test")]
    [MemberData(nameof(ResearchDatasetResultTestCases))]
    public async Task GET_ShouldReturnExpected(string urlParams, Expression<Func<ResearchDataset, bool>> assertPredicate)
    {
        // Arrange
        var apiUrl = $"{_apiBaseUrl}?{urlParams}";

        // Act
        using var response = await _client.GetAsync(apiUrl);

        // Assert
        var researchDatasets = await response.GetResponseObject<IEnumerable<ResearchDataset>>();

        researchDatasets
            .Should()
            .NotBeNullOrEmpty()
            .And.OnlyContain(assertPredicate);
    }

    /// <summary>
    /// Test for running ResearchDataset query against a real elasticsearch instance.
    /// </summary>
    [Theory(Skip = "Integration test")]
    [MemberData(nameof(ResearchDatasetNoResultsTestCases))]
    public async Task GET_ShouldReturnEmpty(string urlParams)
    {
        // Arrange
        var apiUrl = $"{_apiBaseUrl}?{urlParams}";

        // Act
        using var response = await _client.GetAsync(apiUrl);

        // Assert
        var researchDatasets = await response.GetResponseObject<IEnumerable<ResearchDataset>>();

        researchDatasets
            .Should()
            .BeEmpty();
    }

    /// <summary>
    /// Test cases for ResearchDataset search.
    /// First parameter is the query parameters for the search and the other is the predicate the search results must satisfy.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<object[]> ResearchDatasetResultTestCases()
    {
        var testCasesWhichExpectSomethingReturned = new Dictionary<string, Expression<Func<ResearchDataset, bool>>>
        {
            // should find only ResearchDatasets with the given name
            [Name("DOI")] = rds =>
                NamesShouldMatch(rds, "DOI"),

        };

        foreach (var testCase in testCasesWhichExpectSomethingReturned)
        {
            yield return new object[] { testCase.Key, testCase.Value };
        }
    }

    public static IEnumerable<object[]> ResearchDatasetNoResultsTestCases()
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

    public void Dispose()
    {
        _client.Dispose();
    }

    private static bool NamesShouldMatch(ResearchDataset researchDataset, string text)
    {
        return researchDataset != null &&
               (researchDataset.NameFi != null && researchDataset.NameFi.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                researchDataset.NameSv != null && researchDataset.NameSv.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                researchDataset.NameEn != null && researchDataset.NameEn.Contains(text, StringComparison.InvariantCultureIgnoreCase));
    }
}