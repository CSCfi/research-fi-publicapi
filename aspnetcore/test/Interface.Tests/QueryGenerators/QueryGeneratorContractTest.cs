using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;
using CSC.PublicApi.ElasticService.SearchParameters;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Xunit;
using FundingCall = CSC.PublicApi.Service.Models.FundingCall.FundingCall;
using FundingDecision = CSC.PublicApi.Service.Models.FundingDecision.FundingDecision;
using Infrastructure = CSC.PublicApi.Service.Models.Infrastructure.Infrastructure;
using InfrastructureService = CSC.PublicApi.Service.Models.Infrastructure.Service;
using Organization = CSC.PublicApi.Service.Models.Organization.Organization;
using Publication = CSC.PublicApi.Service.Models.Publication.Publication;
using ResearchDataset = CSC.PublicApi.Service.Models.ResearchDataset.ResearchDataset;

namespace CSC.PublicApi.Interface.Tests.QueryGenerators;

public class QueryGeneratorContractTest
{
    public static IEnumerable<object[]> Generators()
    {
        foreach (var kind in Enum.GetValues<Generator>())
            yield return new object[] { kind };
    }

    [Theory]
    [MemberData(nameof(Generators))]
    public void Search_UsesConfiguredIndexAndRequestedPage(Generator kind)
    {
        var requests = Create(kind, "{}", page: 3, size: 7);
        Assert.Equal($"synthetic-{kind.ToString().ToLowerInvariant()}", requests.Index);
        Assert.Equal(14, requests.Search.GetProperty("from").GetInt32());
        Assert.Equal(7, requests.Search.GetProperty("size").GetInt32());
        Assert.False(requests.Search.TryGetProperty("search_after", out _));
        Assert.Equal(ExpectedSort(kind), SortFields(requests.Search));
    }

    [Theory]
    [MemberData(nameof(Generators))]
    public void Search_FirstPageStartsAtZero(Generator kind)
    {
        var requests = Create(kind, "{}");
        Assert.Equal(0, requests.Search.GetProperty("from").GetInt32());
    }

    [Theory]
    [MemberData(nameof(Generators))]
    public void Export_UsesStableSortAndOptionalLongCursorWithoutOffset(Generator kind)
    {
        foreach (var cursor in new long?[] { null, 0, 9007199254740993 })
        {
            var requests = Create(kind, "{}", size: 11, cursor: cursor);
            Assert.Equal(11, requests.Export.GetProperty("size").GetInt32());
            Assert.Equal(new[] { "ExportSortId:asc" }, SortFields(requests.Export));
            Assert.False(requests.Export.TryGetProperty("from", out _));
            if (cursor.HasValue)
                Assert.Equal(new[] { cursor.Value }, requests.Export.GetProperty("search_after")
                    .EnumerateArray().Select(value => value.GetInt64()));
            else
                Assert.False(requests.Export.TryGetProperty("search_after", out _));
            AssertQueryParity(requests.Search, requests.Export);
        }
    }

    [Theory]
    [MemberData(nameof(Generators))]
    public void Defaults_HaveNoUserFiltersAndCountHasNoPaging(Generator kind)
    {
        var requests = Create(kind, "{}");
        if (kind == Generator.Publication)
        {
            using var expected = JsonDocument.Parse(
                """{"bool":{"filter":[{"term":{"IsOrgPublication":{"value":false}}}]}}""");
            Assert.True(JsonElement.DeepEquals(expected.RootElement, requests.Search.GetProperty("query")),
                requests.Search.ToString());
        }
        else
            Assert.False(requests.Search.TryGetProperty("query", out _));
        AssertQueryParity(requests.Search, requests.Count);
        Assert.False(requests.Count.TryGetProperty("size", out _));
        Assert.False(requests.Count.TryGetProperty("from", out _));
        Assert.False(requests.Count.TryGetProperty("sort", out _));
    }

    [Theory]
    [MemberData(nameof(FilterCases))]
    public void Filter_SerializesExpectedClauseAndPreservesSearchCountExportParity(
        Generator kind, string input, string expectedClause, string[] nestedPaths)
    {
        var requests = Create(kind, input);
        using var expected = JsonDocument.Parse(expectedClause);
        var query = requests.Search.GetProperty("query");
        var boolean = query.GetProperty("bool");
        Assert.False(boolean.TryGetProperty("should", out _));
        Assert.Equal(kind == Generator.Publication ? 2 : 1,
            RequiredClauseCount(boolean));
        Assert.Contains(Descendants(query), node => JsonElement.DeepEquals(node, expected.RootElement));
        foreach (var path in nestedPaths)
            Assert.Contains(Descendants(query), node => node.ValueKind == JsonValueKind.Object &&
                node.TryGetProperty("nested", out var nested) &&
                nested.GetProperty("path").GetString() == path);
        Assert.True(JsonElement.DeepEquals(query, requests.Count.GetProperty("query")));
        Assert.True(JsonElement.DeepEquals(query, requests.Export.GetProperty("query")));
    }

    [Theory]
    [InlineData(Generator.Publication, "Id")]
    [InlineData(Generator.Infrastructure, "InfraIdentifier.KeyIdentifierURN")]
    [InlineData(Generator.InfrastructureService, "ServiceIdentifier.KeyIdentifierURN")]
    public void SingleLookup_UsesPublicIdentifierAndConfiguredIndex(Generator kind, string field)
    {
        var requests = Create(kind, "{}", singleId: "synthetic-public-id");
        using var expected = JsonDocument.Parse(
            $$$$"""{"term":{"{{{{field}}}}":{"value":"synthetic-public-id"}}}""");
        Assert.True(JsonElement.DeepEquals(expected.RootElement, requests.Single!.Value.GetProperty("query")));
        Assert.False(requests.Single.Value.TryGetProperty("from", out _));
        Assert.False(requests.Single.Value.TryGetProperty("sort", out _));
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(null, true)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void PublicationVisibility_OnlyAddsRequestedExclusions(bool? showParts, bool hideCoPublications)
    {
        var requests = Create(Generator.Publication, JsonSerializer.Serialize(new
        {
            ShowOrganisationPartofCoPublication = showParts,
            HideCoPublications = hideCoPublications
        }));
        var terms = Descendants(requests.Search).Where(node => node.ValueKind == JsonValueKind.Object &&
            node.TryGetProperty("term", out _)).ToArray();
        Assert.Equal((showParts == true ? 0 : 1) + (hideCoPublications ? 1 : 0), terms.Length);
        Assert.Equal(showParts != true, terms.Any(term => term.GetProperty("term").TryGetProperty("IsOrgPublication", out _)));
        Assert.Equal(hideCoPublications, terms.Any(term => term.GetProperty("term").TryGetProperty("IsCoPublication", out _)));
    }

    [Theory]
    [InlineData(null)]
    [InlineData(false)]
    [InlineData(true)]
    public void DatasetLatestVersion_OnlyTrueRestrictsResults(bool? latest)
    {
        var requests = Create(Generator.ResearchDataset, JsonSerializer.Serialize(new { IsLatestVersion = latest }));
        var terms = Descendants(requests.Search).Where(node => node.ValueKind == JsonValueKind.Object &&
            node.TryGetProperty("term", out _)).ToArray();
        Assert.Equal(latest == true ? 1 : 0, terms.Length);
        if (latest == true)
            Assert.True(terms.Single().GetProperty("term").GetProperty("IsLatestVersion")
                .GetProperty("value").GetBoolean());
    }

    [Fact]
    public void PublicationAuthorNames_ShareOneNestedAuthorQuery()
    {
        var requests = Create(Generator.Publication,
            """{"AuthorFirstNames":"Synthetic","AuthorLastName":"Researcher"}""");
        var nested = requests.Search.GetProperty("query").GetProperty("bool")
            .GetProperty("must").EnumerateArray().Single().GetProperty("nested");
        Assert.Equal("Authors", nested.GetProperty("path").GetString());
        var clauses = nested.GetProperty("query").GetProperty("bool").GetProperty("must");
        Assert.Equal(2, clauses.GetArrayLength());
        Assert.Equal("Synthetic", clauses[0].GetProperty("match").GetProperty("Authors.FirstNames")
            .GetProperty("query").GetString());
        Assert.Equal("Researcher", clauses[1].GetProperty("match").GetProperty("Authors.LastName")
            .GetProperty("query").GetString());
    }

    [Theory]
    [InlineData(Generator.FundingCall)]
    [InlineData(Generator.FundingDecision)]
    [InlineData(Generator.Infrastructure)]
    [InlineData(Generator.InfrastructureService)]
    [InlineData(Generator.Publication)]
    [InlineData(Generator.ResearchDataset)]
    public void CombinedFilters_PreserveEveryClauseUnderRequiredBooleanBranches(Generator kind)
    {
        var cases = FilterCases().Where(row => (Generator)row[0] == kind).ToArray();
        var input = new JsonObject();
        foreach (var row in cases)
        foreach (var property in JsonNode.Parse((string)row[1])!.AsObject())
            input[property.Key] = property.Value!.DeepClone();
        var requests = Create(kind, input.ToJsonString());
        var boolean = requests.Search.GetProperty("query").GetProperty("bool");
        Assert.False(boolean.TryGetProperty("should", out _));
        // Publication merges the two author names and adds its default visibility
        // filter. Infrastructure supplies both roadmap values; the last wins.
        var expectedCount = cases.Length - (kind == Generator.Infrastructure ? 1 : 0);
        Assert.Equal(expectedCount, RequiredClauseCount(boolean));
        foreach (var row in cases)
        {
            using var parameter = JsonDocument.Parse((string)row[1]);
            // Both names intentionally use one correlated nested clause.
            if (kind == Generator.Publication && parameter.RootElement.EnumerateObject().Single().Name
                is "AuthorFirstNames" or "AuthorLastName")
                continue;
            using var expected = JsonDocument.Parse((string)row[2]);
            if (kind == Generator.Infrastructure &&
                parameter.RootElement.TryGetProperty("FinlandRoadmap", out var roadmap) && !roadmap.GetBoolean())
                continue;
            Assert.Contains(Descendants(boolean), node => JsonElement.DeepEquals(node, expected.RootElement));
        }
        AssertQueryParity(requests.Search, requests.Count);
        AssertQueryParity(requests.Search, requests.Export);
    }

    [Theory]
    [InlineData(Generator.FundingCall, "Name")]
    [InlineData(Generator.FundingCall, "FoundationName")]
    [InlineData(Generator.FundingCall, "FoundationBusinessId")]
    [InlineData(Generator.FundingCall, "Category")]
    [InlineData(Generator.FundingDecision, "Name")]
    [InlineData(Generator.FundingDecision, "Description")]
    [InlineData(Generator.FundingDecision, "FunderName")]
    [InlineData(Generator.FundingDecision, "TypeOfFunding")]
    [InlineData(Generator.Organization, "Name")]
    [InlineData(Generator.Infrastructure, "InfraName")]
    [InlineData(Generator.InfrastructureService, "ServiceName")]
    [InlineData(Generator.Publication, "Name")]
    [InlineData(Generator.Publication, "AuthorFirstNames")]
    [InlineData(Generator.Publication, "AuthorLastName")]
    [InlineData(Generator.ResearchDataset, "Name")]
    [InlineData(Generator.ResearchDataset, "Description")]
    public void WhitespaceText_DoesNotAddAQuery(Generator kind, string parameter)
    {
        foreach (var value in new[] { "", " \t " })
        {
            var input = JsonSerializer.Serialize(new Dictionary<string, string> { [parameter] = value });
            AssertQueryParity(Create(kind, "{}").Search, Create(kind, input).Search);
        }
    }

    [Theory]
    [InlineData(Generator.Publication, "CreatedFrom", "Created", "gte")]
    [InlineData(Generator.Publication, "CreatedTo", "Created", "lte")]
    [InlineData(Generator.Publication, "ModifiedFrom", "Modified", "gte")]
    [InlineData(Generator.Publication, "ModifiedTo", "Modified", "lte")]
    [InlineData(Generator.ResearchDataset, "DateFrom", "Created", "gte")]
    [InlineData(Generator.ResearchDataset, "DateTo", "Created", "lte")]
    public void DateBounds_AreInclusiveAndRetainedInCountAndExport(
        Generator kind, string parameter, string field, string comparison)
    {
        var input = JsonSerializer.Serialize(new Dictionary<string, string> { [parameter] = "2025-01-02" });
        var requests = Create(kind, input);
        var range = Descendants(requests.Search).Single(node => node.ValueKind == JsonValueKind.Object &&
            node.TryGetProperty("range", out _)).GetProperty("range").GetProperty(field);
        Assert.StartsWith("2025-01-02", range.GetProperty(comparison).GetString());
        Assert.Single(range.EnumerateObject());
        AssertQueryParity(requests.Search, requests.Count);
        AssertQueryParity(requests.Search, requests.Export);
    }

    [Theory]
    [InlineData("PublicationYear")]
    [InlineData("ReportingYear")]
    public void PublicationYear_UsesInclusiveYearRange(string parameter)
    {
        var requests = Create(Generator.Publication,
            JsonSerializer.Serialize(new Dictionary<string, string> { [parameter] = "2025" }));
        var range = Descendants(requests.Search).Single(node => node.ValueKind == JsonValueKind.Object &&
            node.TryGetProperty("range", out _)).GetProperty("range").GetProperty(parameter);
        Assert.Equal("2025", range.GetProperty("gte").GetString());
        Assert.Equal("2025", range.GetProperty("lte").GetString());
        Assert.Equal("yyyy", range.GetProperty("format").GetString());
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void FundingCallDateOverlap_IncludesOpenEndedCalls(bool from, bool to)
    {
        var requests = Create(Generator.FundingCall, JsonSerializer.Serialize(new
        {
            DateFrom = from ? "2025-01-02" : null,
            DateTo = to ? "2025-12-30" : null
        }));
        var rangeNodes = Descendants(requests.Search).Where(node => node.ValueKind == JsonValueKind.Object &&
            node.TryGetProperty("range", out _)).Select(node => node.GetProperty("range")).ToArray();
        Assert.Equal(2, rangeNodes.Length);
        var open = rangeNodes.Single(node => node.TryGetProperty("CallProgrammeOpenDate", out _))
            .GetProperty("CallProgrammeOpenDate");
        var due = rangeNodes.Single(node => node.TryGetProperty("CallProgrammeDueDate", out _))
            .GetProperty("CallProgrammeDueDate");
        Assert.StartsWith(to ? "2025-12-30" : "9999-12-31", open.GetProperty("lte").GetString());
        Assert.StartsWith(from ? "2025-01-02" : "0001-01-01", due.GetProperty("gte").GetString());
        var missingBranches = Descendants(requests.Search).Where(node => node.ValueKind == JsonValueKind.Object &&
            node.TryGetProperty("must_not", out _)).ToArray();
        Assert.Equal(2, missingBranches.Length);
        foreach (var field in new[] { "CallProgrammeOpenDate", "CallProgrammeDueDate" })
            Assert.Contains(missingBranches, branch => Descendants(branch).Any(node =>
                node.ValueKind == JsonValueKind.Object && node.TryGetProperty("exists", out var exists) &&
                exists.GetProperty("field").GetString() == field));
        AssertQueryParity(requests.Search, requests.Count);
        AssertQueryParity(requests.Search, requests.Export);
    }

    [Fact]
    public void FundingDecisionStartDate_IncludesBoundaryAndMissingDates()
    {
        var requests = Create(Generator.FundingDecision, """{"FundingStartYearFrom":"2025-01-02"}""");
        var alternatives = requests.Search.GetProperty("query").GetProperty("bool")
            .GetProperty("filter")[0].GetProperty("bool").GetProperty("should")[0]
            .GetProperty("bool").GetProperty("should");
        Assert.Equal(2, alternatives.GetArrayLength());
        var range = alternatives[0].GetProperty("range").GetProperty("FundingStartDate");
        Assert.StartsWith("2025-01-02", range.GetProperty("gte").GetString());
        Assert.Single(range.EnumerateObject());
        var missing = alternatives[1].GetProperty("bool").GetProperty("must_not")[0]
            .GetProperty("exists").GetProperty("field").GetString();
        Assert.Equal("FundingStartDate", missing);
        AssertQueryParity(requests.Search, requests.Count);
        AssertQueryParity(requests.Search, requests.Export);
    }

    private static int RequiredClauseCount(JsonElement boolean) =>
        (boolean.TryGetProperty("must", out var must) ? must.GetArrayLength() : 0) +
        (boolean.TryGetProperty("filter", out var filter) ? filter.GetArrayLength() : 0);

    private static void AssertQueryParity(JsonElement first, JsonElement second)
    {
        var firstHasQuery = first.TryGetProperty("query", out var firstQuery);
        Assert.Equal(firstHasQuery, second.TryGetProperty("query", out var secondQuery));
        if (firstHasQuery)
            Assert.True(JsonElement.DeepEquals(firstQuery, secondQuery), second.ToString());
    }

    public static IEnumerable<object[]> FilterCases()
    {
        foreach (var kind in new[] { Generator.FundingCall, Generator.FundingDecision, Generator.ResearchDataset })
        {
            yield return Multi(kind, "Name", "NameFi", "NameSv", "NameEn");
            if (kind != Generator.FundingCall)
                yield return Multi(kind, "Description", "DescriptionFi", "DescriptionSv", "DescriptionEn");
        }
        yield return Multi(Generator.FundingCall, "FoundationName",
            new[] { "Foundations.NameFi", "Foundations.NameSv", "Foundations.NameEn" }, "Foundations");
        yield return Term(Generator.FundingCall, "FoundationBusinessId", "Foundations.BusinessId", "Foundations");
        yield return Term(Generator.FundingCall, "Category", "Categories.Code");
        yield return Multi(Generator.FundingDecision, "FunderName", "Funder.NameFi", "Funder.NameSv", "Funder.NameEn");
        yield return Multi(Generator.FundingDecision, "FundedOrganization",
            "FundingReceivers.Organization.NameFi", "FundingReceivers.Organization.NameSv", "FundingReceivers.Organization.NameEn");
        foreach (var (parameter, field) in new[]
        {
            ("FunderProjectNumber", "FunderProjectNumber"), ("FunderId", "Funder.Pids.Content"),
            ("FundedPersonOrcid", "FundingReceivers.Person.OrcId"),
            ("FundedOrganizationId", "FundingReceivers.Organization.Pids.Content"), ("TypeOfFunding", "TypeOfFunding.Code")
        })
            yield return Term(Generator.FundingDecision, parameter, field);
        foreach (var (parameter, field) in new[]
        {
            ("FundedPersonFirstName", "FundingReceivers.Person.FirstNames"),
            ("FundedPersonLastName", "FundingReceivers.Person.LastName")
        })
            yield return Case(Generator.FundingDecision, parameter, "Synthetic",
                $$$$"""{"query_string":{"fields":["{{{{field}}}}"],"query":"Synthetic","type":"phrase_prefix"}}""");

        foreach (var (parameter, field, operation) in new[]
        {
            ("Name", "Name", "match_phrase"), ("OriginalPublicationId", "OriginalPublicationId", "match"),
            ("AuthorsText", "AuthorsText", "match"), ("ConferenceName", "ConferenceName", "match"),
            ("JournalName", "JournalName", "match"), ("ParentPublicationName", "ParentPublication.Name", "match"),
            ("ParentPublicationPublisher", "ParentPublication.Publisher", "match"),
            ("PublisherName", "PublisherName", "match"), ("Keywords", "Keywords.Value", "match"),
            ("AuthorFirstNames", "Authors.FirstNames", "match"), ("AuthorLastName", "Authors.LastName", "match"),
            ("AuthorOrcId", "Authors.Orcid", "match")
        })
            yield return Text(Generator.Publication, parameter, field, operation,
                parameter.StartsWith("Author", StringComparison.Ordinal) && parameter != "AuthorsText"
                    ? new[] { "Authors" } : Array.Empty<string>());
        foreach (var (parameter, field) in new[]
        {
            ("OrganizationId", "Organizations.Id"), ("OrganizationUnitId", "Organizations.Units.Id"),
            ("TypeCode", "Type.Code"), ("Issn", "Issn"), ("Isbn", "Isbn"), ("JufoCode", "JufoCode"),
            ("JufoCodeRecorded", "JufoCodeRecorded"), ("Doi", "Doi"), ("JufoClass", "JufoClass.Code"),
            ("JufoClassRecorded", "JufoClassRecorded.Code"), ("PublisherOpenAccess", "PublisherOpenAccess.Code"),
            ("Status", "Status.Code")
        })
            yield return Term(Generator.Publication, parameter, field);

        foreach (var (parameter, field) in new[]
        {
            ("Keywords", "Keywords.Value"), ("SubjectHeadings", "SubjectHeadings.Value"),
            ("PersonName", "Contributors.Person.Name")
        })
            yield return Text(Generator.ResearchDataset, parameter, field, "match");
        yield return Case(Generator.ResearchDataset, "OrganizationName", "Synthetic",
            """{"multi_match":{"fields":["Contributors.Organization.NameFi","Contributors.Organization.NameSv","Contributors.Organization.NameEn","Contributors.Organization.NameVariants"],"query":"Synthetic"}}""");
        foreach (var (parameter, field) in new[]
        {
            ("Access", "AccessType.Code"), ("License", "License.Code"), ("RelatedDatasetId", "DatasetRelations.Id"),
            ("OrganizationId", "Contributors.Organization.Id"), ("ResearchDataCatalog", "ResearchDataCatalog.Id")
        })
            yield return Term(Generator.ResearchDataset, parameter, field);
        foreach (var (parameter, field) in new[] { ("FieldOfScience", "FieldsOfScience.Code"), ("Language", "Languages.Code") })
            yield return Case(Generator.ResearchDataset, parameter, new[] { "one", "two" },
                $$$$"""{"terms":{"{{{{field}}}}":["one","two"]}}""");

        foreach (var kind in new[] { Generator.Infrastructure, Generator.InfrastructureService })
        {
            var service = kind == Generator.InfrastructureService;
            var prefix = service ? "Service" : "Infra";
            yield return Text(kind, "KeyIdentifierUrn", $"{prefix}Identifier.KeyIdentifierURN", "match_phrase");
            yield return Text(kind, "LocalIdentifier", $"{prefix}Identifier.LocalIdentifier", "match_phrase");
            yield return Text(kind, "OtherPersistentIdentifier", $"{prefix}Identifier.OtherPid.PidContent",
                "match_phrase", $"{prefix}Identifier.OtherPid");
            yield return Text(kind, $"{prefix}Name", $"{prefix}Name.TextContent", "match_phrase", $"{prefix}Name");
            yield return Text(kind, $"{prefix}Description", $"{prefix}Description.TextContent", "match_phrase", $"{prefix}Description");
            yield return Case(kind, $"{prefix}StartsOnYear", 2020,
                $$$$"""{"term":{"{{{{prefix}}}}StartsOn.Year":{"value":2020}}}""", $"{prefix}StartsOn");
            yield return Case(kind, $"{prefix}EndsOnYear", 2030,
                $$$$"""{"term":{"{{{{prefix}}}}EndsOn.Year":{"value":2030}}}""", $"{prefix}EndsOn");
        }
        yield return Text(Generator.Infrastructure, "InfraAcronym", "InfraAcronym", "match_phrase");
        yield return Term(Generator.Infrastructure, "EsfriCode", "ESFRICodes.CodeValue", "ESFRICodes");
        yield return Term(Generator.Infrastructure, "FieldOfScience", "FieldOfScience.CodeValue", "FieldOfScience");
        foreach (var value in new[] { false, true })
            yield return Case(Generator.Infrastructure, "FinlandRoadmap", value,
                $$$$"""{"term":{"FinlandRoadmap":{"value":{{{{value.ToString().ToLowerInvariant()}}}}}}}""");
        yield return Term(Generator.Infrastructure, "InfraResponsibleOrganizationId",
            "InfraResponsibleOrganization.OrganizationIdentifier.PidContent",
            "InfraResponsibleOrganization", "InfraResponsibleOrganization.OrganizationIdentifier");
        yield return Term(Generator.Infrastructure, "InfraParticipatingOrganizationsId",
            "InfraParticipatingOrganizations.OrganizationIdentifier.PidContent",
            "InfraParticipatingOrganizations", "InfraParticipatingOrganizations.OrganizationIdentifier");
        yield return Term(Generator.Infrastructure, "VisitingAddressCountryCode",
            "InfraContactInformation.VisitingAddress.Country.CodeValue", "InfraContactInformation");
        yield return Text(Generator.InfrastructureService, "IsPartOfInfrastructureURN",
            "IsPartOfInfrastructure.InfraIdentifier.KeyIdentifierURN", "match_phrase", "IsPartOfInfrastructure");
        yield return Term(Generator.InfrastructureService, "IsPartOfInfrastructureInfraResponsibleOrganizationId",
            "IsPartOfInfrastructure.InfraResponsibleOrganization.OrganizationIdentifier.PidContent",
            "IsPartOfInfrastructure", "IsPartOfInfrastructure.InfraResponsibleOrganization.OrganizationIdentifier");
        yield return Term(Generator.InfrastructureService, "IsPartOfInfrastructureInfraParticipatingOrganizationsId",
            "IsPartOfInfrastructure.InfraParticipatingOrganizations.OrganizationIdentifier.PidContent",
            "IsPartOfInfrastructure", "IsPartOfInfrastructure.InfraParticipatingOrganizations.OrganizationIdentifier");
        yield return Case(Generator.InfrastructureService, "ServiceEndsByYear", 2030,
            """{"range":{"ServiceEndsOn.Year":{"lte":2030.0}}}""", "ServiceEndsOn");
    }

    private static object[] Term(Generator kind, string parameter, string field, params string[] paths) =>
        Case(kind, parameter, "Synthetic", $$$$"""{"term":{"{{{{field}}}}":{"value":"Synthetic"}}}""", paths);

    private static object[] Text(Generator kind, string parameter, string field, string operation, params string[] paths) =>
        Case(kind, parameter, "Synthetic", $$$$"""{"{{{{operation}}}}":{"{{{{field}}}}":{"query":"Synthetic"}}}""", paths);

    private static object[] Multi(Generator kind, string parameter, params string[] fields) =>
        Multi(kind, parameter, fields, Array.Empty<string>());

    private static object[] Multi(Generator kind, string parameter, string[] fields, params string[] paths) =>
        Case(kind, parameter, "Synthetic",
            $$$$"""{"multi_match":{"fields":{{{{JsonSerializer.Serialize(fields)}}}},"query":"Synthetic","type":"phrase_prefix"}}""", paths);

    private static object[] Case(Generator kind, string parameter, object value, string expected, params string[] paths) =>
        new object[] { kind, JsonSerializer.Serialize(new Dictionary<string, object> { [parameter] = value }), expected, paths };

    private static string[] ExpectedSort(Generator kind) => kind switch
    {
        Generator.FundingCall => new[] { "CallProgrammeDueDate:asc", "ExportSortId:asc" },
        Generator.FundingDecision => new[] { "FundingStartDate:desc", "ExportSortId:asc" },
        Generator.Publication => new[] { "PublicationYear:desc", "ExportSortId:asc" },
        Generator.ResearchDataset => new[] { "Created:desc", "ExportSortId:asc" },
        Generator.Organization => Array.Empty<string>(),
        _ => new[] { "ExportSortId:asc" }
    };

    private static string[] SortFields(JsonElement request) =>
        request.TryGetProperty("sort", out var sort)
            ? sort.EnumerateArray().Select(item => item.EnumerateObject().Single())
                .Select(field => $"{field.Name}:{field.Value.GetProperty("order").GetString()}").ToArray()
            : Array.Empty<string>();

    private static IEnumerable<JsonElement> Descendants(JsonElement node)
    {
        yield return node;
        var children = node.ValueKind switch
        {
            JsonValueKind.Object => node.EnumerateObject().Select(property => property.Value),
            JsonValueKind.Array => node.EnumerateArray().AsEnumerable(),
            _ => Enumerable.Empty<JsonElement>()
        };
        foreach (var child in children)
        foreach (var descendant in Descendants(child))
            yield return descendant;
    }

    private static Requests Create(Generator kind, string input, int page = 1, int size = 10,
        long? cursor = null, string? singleId = null) => kind switch
    {
        Generator.FundingCall => Create<FundingCallSearchParameters, FundingCall>(kind, input, page, size, cursor, singleId),
        Generator.FundingDecision => Create<FundingDecisionSearchParameters, FundingDecision>(kind, input, page, size, cursor, singleId),
        Generator.Infrastructure => Create<InfrastructureSearchParameters, Infrastructure>(kind, input, page, size, cursor, singleId),
        Generator.InfrastructureService => Create<InfrastructureServiceSearchParameters, InfrastructureService>(kind, input, page, size, cursor, singleId),
        Generator.Organization => Create<OrganizationSearchParameters, Organization>(kind, input, page, size, cursor, singleId),
        Generator.Publication => Create<PublicationSearchParameters, Publication>(kind, input, page, size, cursor, singleId),
        Generator.ResearchDataset => Create<ResearchDatasetSearchParameters, ResearchDataset>(kind, input, page, size, cursor, singleId),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };

    private static Requests Create<TParameters, TModel>(Generator kind, string input,
        int page, int size, long? cursor, string? singleId) where TModel : class
    {
        var index = $"synthetic-{kind.ToString().ToLowerInvariant()}";
        var services = new ServiceCollection();
        services.AddSingleton(new IndexNameSettings { [typeof(TModel).FullName!] = index });
        services.AddElasticSearch(new ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string?> { ["ElasticSearch:Url"] = "http://127.0.0.1:1" }).Build());
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var generator = scope.ServiceProvider.GetRequiredService<IQueryGenerator<TParameters, TModel>>();
        var client = scope.ServiceProvider.GetRequiredService<IElasticClient>();
        var parameters = JsonSerializer.Deserialize<TParameters>(input)
            ?? throw new InvalidOperationException("Test parameters must not be null.");
        var search = generator.GenerateQuery(parameters, page, size)(new SearchDescriptor<TModel>());
        var count = generator.GenerateCountQuery(parameters)(new CountDescriptor<TModel>());
        var export = generator.GenerateQuerySearchAfter(parameters, size, cursor)(new SearchDescriptor<TModel>());
        Assert.Equal((Indices)index, search.Index);
        Assert.Equal((Indices)index, count.Index);
        Assert.Equal((Indices)index, export.Index);
        JsonElement? single = null;
        if (singleId != null)
        {
            var request = generator.GenerateSingleQuery(singleId)(new SearchDescriptor<TModel>());
            Assert.Equal((Indices)index, request.Index);
            single = Serialize(request);
        }
        return new Requests(index, Serialize(search), Serialize(count), Serialize(export), single);

        JsonElement Serialize<T>(T request)
        {
            using var document = JsonDocument.Parse(client.RequestResponseSerializer.SerializeToString(request));
            return document.RootElement.Clone();
        }
    }

    private sealed record Requests(string Index, JsonElement Search, JsonElement Count, JsonElement Export, JsonElement? Single);

    public enum Generator
    {
        FundingCall, FundingDecision, Infrastructure, InfrastructureService, Organization, Publication, ResearchDataset
    }
}
