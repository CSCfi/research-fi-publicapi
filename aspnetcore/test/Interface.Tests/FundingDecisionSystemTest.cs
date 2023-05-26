using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.Interface.Tests.TestHelpers;
using CSC.PublicApi.Service.Models.FundingDecision;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CSC.PublicApi.Interface.Tests;

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
    /// Test for running funding decision query against a real elasticsearch instance.
    /// </summary>
    [Theory(Skip = "Integration test")]
    [MemberData(nameof(FundingDecisionResultTestCases))]
    public async void GET_ShouldReturnExpected(string urlParams, Expression<Func<FundingDecision, bool>> assertPredicate)
    {
        // Arrange
        var apiUrl = $"{_apiBaseUrl}?{urlParams}";

        // Act
        var response = await _client.GetAsync(apiUrl);

        // Assert
        var fundingDecisions = await response.GetResponseObject<IEnumerable<FundingDecision>>();

        using (new AssertionScope())
        {
            AssertionOptions.FormattingOptions.MaxDepth = 7;

            fundingDecisions
                .Should()
                .NotBeNullOrEmpty()
                .And.OnlyContain(assertPredicate);
        }
    }

    /// <summary>
    /// Test for running funding decision query against a real elasticsearch instance.
    /// </summary>
    [Theory(Skip = "Integration test")]
    [MemberData(nameof(FundingDecisionNoResultsTestCases))]
    public async void GET_ShouldReturnEmpty(string urlParams)
    {
        // Arrange
        var apiUrl = $"{_apiBaseUrl}?{urlParams}";

        // Act
        var response = await _client.GetAsync(apiUrl);

        // Assert
        var fundingDecisions = await response.GetResponseObject<IEnumerable<FundingDecision>>();

        fundingDecisions
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
            // should find only decisions with the given name
            [Name("Architectures")] = fd => NamesShouldMatch(fd, "Architectures"),
            // should find only decisions with the given name, framework programme should be correct
            [Name("Plasmasphere Ionosphere Thermosphere Integrated Research Environment")] = fd =>
                NamesShouldMatch(fd, "Plasmasphere Ionosphere Thermosphere Integrated Research Environment") &&
                fd.FrameworkProgramme != null && fd.FrameworkProgramme.NameEn == "Horizon 2020 Framework Programme",
            // should find decisions with the description
            [Description("tekoäly")] = fd => DescriptionShouldMatch(fd, "tekoäly"),
            // should find only decisions with the given funder name
            [FunderName("akatemia")] = fd => FunderNamesShouldMatch(fd, "akatemia"),
            // should find only decisions with the given funder id
            [FunderId("999517586")] = fd => FunderIdShouldMatch(fd, "999517586"),
            // should find only decisions with the given project number
            [FunderProjectNumber("345790")] = fd => FunderProjectNumberShouldMatch(fd, "345790"),
            // should find only decisions with funding start year >= x
            // should match organization consortium names
            // should match organization consortium id
            // should match with funding group person's first name
            // should match with funding group person's last name
            // should match with funding group person's orcid
            [TypeOfFunding("ERC-STG")] = fd => TypeOfFundingShouldMatch(fd, "ERC-STG"),
            // combinations should work
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

    private static string Name(string name)
    {
        return $"name={name}";
    }

    private static bool NamesShouldMatch(FundingDecision fd, string text)
    {
        return fd != null &&
               (fd.NameFi != null && fd.NameFi.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                fd.NameSv != null && fd.NameSv.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                fd.NameEn != null && fd.NameEn.Contains(text, StringComparison.InvariantCultureIgnoreCase));
    }

    private static string Description(string description)
    {
        return $"description={description}";
    }

    private static bool DescriptionShouldMatch(FundingDecision fd, string text)
    {
        return fd != null &&
               (fd.DescriptionFi != null && fd.DescriptionFi.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                fd.DescriptionSv != null && fd.DescriptionSv.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                fd.DescriptionEn != null && fd.DescriptionEn.Contains(text, StringComparison.InvariantCultureIgnoreCase));
    }

    private static string FunderName(string name)
    {
        return $"fundername={name}";
    }

    private static bool FunderNamesShouldMatch(FundingDecision fd, string text)
    {
        return fd.Funder != null &&
               (fd.Funder.NameFi != null && fd.Funder.NameFi.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                fd.Funder.NameSv != null && fd.Funder.NameSv.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                fd.Funder.NameEn != null && fd.Funder.NameEn.Contains(text, StringComparison.InvariantCultureIgnoreCase));
    }

    private static string FunderId(string name)
    {
        return $"funderid={name}";
    }

    private static bool FunderIdShouldMatch(FundingDecision fd, string text)
    {
        return fd.Funder != null && fd.Funder.Pids != null && fd.Funder.Pids.Any(id => id.Content == text);
    }

    private static string FunderProjectNumber(string projectNumber)
    {
        return $"funderprojectnumber={projectNumber}";
    }

    private static bool FunderProjectNumberShouldMatch(FundingDecision fd, string text)
    {
        return fd.FunderProjectNumber == text;
    }

    private static string FundingStartYearFrom(int year)
    {
        return $"fundingstartyearfrom={year}";
    }

    private static string FundedOrganization(string name)
    {
        return $"fundedorganization={name}";
    }

    private static string FundedOrganizationId(string id)
    {
        return $"fundedorganizationid={id}";
    }

    private static string FundedPersonFirstName(string name)
    {
        return $"fundedpersonfirstname={name}";
    }

    private static string FundedPersonLastName(string name)
    {
        return $"fundedpersonlastname={name}";
    }
   

    private static string FundedPersonOrcid(string orcid)
    {
        return $"fundedpersonorcid={orcid}";
    }

    private static string TypeOfFunding(string type)
    {
        return $"typeoffunding={type}";
    }

    private static bool TypeOfFundingShouldMatch(FundingDecision fd, string text)
    {
        return fd.TypeOfFunding != null && fd.TypeOfFunding.Code == text;
    }

}