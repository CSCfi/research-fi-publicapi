using CSC.PublicApi.DataAccess;
using CSC.PublicApi.Interface;
using CSC.PublicApi.Interface.Models.Organization;
using CSC.PublicApi.Tests.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using Xunit;

namespace CSC.PublicApi.Tests;

public class OrganizationSystemTest : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly ApiDbContext _dbContext;
    private readonly HttpClient _client;
    private readonly string _apiBaseUrl = "v1/Organization";

    public OrganizationSystemTest(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        _client = _factory.CreateClient();
    }

    /// <summary>
    /// Test for running Organization query against a real elasticsearch instance.
    /// </summary>
    [Theory]
    [MemberData(nameof(OrganizationResultTestCases))]
    public async void GET_ShouldReturnExpected(string urlParams, Expression<Func<Organization, bool>> assertPredicate)
    {
        // Arrange
        var apiUrl = $"{_apiBaseUrl}?{urlParams}";

        // Act
        var response = await _client.GetAsync(apiUrl);

        // Assert
        var organizations = await response.GetResponseObject<IEnumerable<Organization>>();

        organizations
            .Should()
            .NotBeNullOrEmpty()
            .And.OnlyContain(assertPredicate);
    }

    /// <summary>
    /// Test for running Organization query against a real elasticsearch instance.
    /// </summary>
    [Theory]
    [MemberData(nameof(OrganizationNoResultsTestCases))]
    public async void GET_ShouldReturnEmpty(string urlParams)
    {
        // Arrange
        var apiUrl = $"{_apiBaseUrl}?{urlParams}";

        // Act
        var response = await _client.GetAsync(apiUrl);

        // Assert
        var organizations = await response.GetResponseObject<IEnumerable<Organization>>();

        organizations
            .Should()
            .BeEmpty();
    }

    /// <summary>
    /// Test cases for Organization search.
    /// First parameter is the query parameters for the search and the other is the predicate the search results must satisfy.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<object[]> OrganizationResultTestCases()
    {
        var testCasesWhichExpectSomethingReturned = new Dictionary<string, Expression<Func<Organization, bool>>>
        {
            // should find only organizations with the given name
            [Name("turku")] = fc =>
                NamesShouldMatch(fc, "turku"),

        };

        foreach (var testCase in testCasesWhichExpectSomethingReturned)
        {
            yield return new object[] { testCase.Key, testCase.Value };
        }
    }

    public static IEnumerable<object[]> OrganizationNoResultsTestCases()
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

    private static bool NamesShouldMatch(Organization organization, string text)
    {
        return organization != null &&
               (organization.NameFi != null && organization.NameFi.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                organization.NameSv != null && organization.NameSv.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                organization.NameEn != null && organization.NameEn.Contains(text, StringComparison.InvariantCultureIgnoreCase));
    }
}