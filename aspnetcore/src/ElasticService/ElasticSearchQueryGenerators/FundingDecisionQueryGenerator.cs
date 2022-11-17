using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models.FundingDecision;
using Nest;
using Id = CSC.PublicApi.Service.Models.FundingDecision.Id;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

public class FundingDecisionQueryGenerator : QueryGeneratorBase<FundingDecisionSearchParameters, FundingDecision>
{
    public FundingDecisionQueryGenerator(IndexNameSettings configuration) : base(configuration)
    {
    }

    protected override Func<QueryContainerDescriptor<FundingDecision>, QueryContainer> GenerateQueryForIndex(
        FundingDecisionSearchParameters parameters)
    {
        var subQueries = CreateSubQueries(parameters);
        var filters = CreateFilters(parameters);

        return queryDescriptor => queryDescriptor
            .Bool(boolDescriptor => boolDescriptor
                .Must(subQueries)
                .Filter(filters)
            );
    }

    private static IEnumerable<Func<QueryContainerDescriptor<FundingDecision>, QueryContainer>> CreateSubQueries(
        FundingDecisionSearchParameters parameters)
    {
        var subQueries = new List<Func<QueryContainerDescriptor<FundingDecision>, QueryContainer>>();

        // When searching with Name, search from Fi,Sv,En names.
        if (!string.IsNullOrWhiteSpace(parameters.Name))
        {
            subQueries.Add(t => t.MultiMatch(query => query
                .Type(TextQueryType.PhrasePrefix)
                .Fields(r => r
                    .Field(f => f.NameFi)
                    .Field(f => f.NameSv)
                    .Field(f => f.NameEn))
                .Query(parameters.Name)));
        }

        // When searching with Description, search from Fi,Sv,En descriptions.
        if (!string.IsNullOrWhiteSpace(parameters.Description))
        {
            subQueries.Add(t => t.MultiMatch(query => query
                .Type(TextQueryType.PhrasePrefix)
                .Fields(r => r
                    .Field(f => f.DescriptionFi)
                    .Field(f => f.DescriptionSv)
                    .Field(f => f.DescriptionEn))
                .Query(parameters.Description)));
        }

        // When searching with FoundationName, search from Fi,Sv,En names.
        if (!string.IsNullOrWhiteSpace(parameters.FunderName))
        {
            subQueries.Add(t => t.Nested(query => query
                .Path(p => p.Funder)
                .Query(q => q.MultiMatch(multi => multi
                    .Type(TextQueryType.PhrasePrefix)
                    .Fields(r => r
                        .Field(f => f.Funder.Suffix(nameof(Funder.NameFi)))
                        .Field(f => f.Funder.Suffix(nameof(Funder.NameSv)))
                        .Field(f => f.Funder.Suffix(nameof(Funder.NameEn))))
                    .Query(parameters.FunderName)
                ))));
        }

        // Searching with funder id requires exact match.
        if (!string.IsNullOrWhiteSpace(parameters.FunderId))
        {
            subQueries.Add(t => t.Nested(query => query
                .Path(p => p.Funder!.Ids)
                .Query(q => q.Term(term => term
                    .Field(f => f.Funder!.Ids.Suffix(nameof(Id.Content)))
                    .Value(parameters.FunderId)
                ))));
        }

        // Searching with project number requires exact match.
        if (!string.IsNullOrWhiteSpace(parameters.FunderProjectNumber))
        {
            subQueries.Add(t => t.Term(fd => fd.FunderProjectNumber, parameters.FunderProjectNumber));
        }

        // When searching with FundedOrganisation, search from organization consortia Fi,Sv,En names.
        if (!string.IsNullOrWhiteSpace(parameters.FundedOrganization))
        {
            subQueries.Add(t => t.Nested(query => query
                .Path(p => p.OrganizationConsortia)
                .Query(q => q.MultiMatch(multi => multi
                    .Type(TextQueryType.PhrasePrefix)
                    .Fields(r => r
                        .Field(f => f.OrganizationConsortia.Suffix(nameof(OrganizationConsortium.NameFi)))
                        .Field(f => f.OrganizationConsortia.Suffix(nameof(OrganizationConsortium.NameSv)))
                        .Field(f => f.OrganizationConsortia.Suffix(nameof(OrganizationConsortium.NameEn))))
                    .Query(parameters.FundedOrganization)
                ))));
        }

        // Searching with funded organization id requires exact match.
        if (!string.IsNullOrWhiteSpace(parameters.FundedOrganizationId))
        {
            subQueries.Add(t => t.Nested(query => query
                .Path(p => p.OrganizationConsortia.Suffix(nameof(OrganizationConsortium.Ids)))
                .Query(q => q.Term(term => term
                    .Field(f => f.OrganizationConsortia.Suffix(nameof(OrganizationConsortium.Ids))
                        .Suffix(nameof(Id.Content)))
                    .Value(parameters.FundedOrganizationId)
                ))));
        }

        // Searching with funded person first name
        if (!string.IsNullOrWhiteSpace(parameters.FundedPersonFirstName))
        {
            subQueries.Add(t => t.Nested(query => query
                .Path(p => p.FundingGroupPerson)
                .Query(q => q.QueryString(multi => multi
                    .Type(TextQueryType.PhrasePrefix)
                    .Fields(fields =>
                        fields.Field(f => f.FundingGroupPerson.Suffix(nameof(FundingGroupPerson.FirstNames))))
                    .Query(parameters.FundedPersonFirstName)
                ))));
        }

        // Searching with funded person last name
        if (!string.IsNullOrWhiteSpace(parameters.FundedPersonLastName))
        {
            subQueries.Add(t => t.Nested(query => query
                .Path(p => p.FundingGroupPerson)
                .Query(q => q.QueryString(multi => multi
                    .Type(TextQueryType.PhrasePrefix)
                    .Fields(fields =>
                        fields.Field(f => f.FundingGroupPerson.Suffix(nameof(FundingGroupPerson.LastName))))
                    .Query(parameters.FundedPersonLastName)
                ))));
        }

        // Searching with funded organization id requires exact match.
        if (!string.IsNullOrWhiteSpace(parameters.FundedPersonOrcid))
        {
            subQueries.Add(t => t.Nested(query => query
                .Path(p => p.FundingGroupPerson)
                .Query(q => q.Term(term => term
                    .Field(f => f.FundingGroupPerson.Suffix(nameof(FundingGroupPerson.OrcId)))
                    .Value(parameters.FundedPersonOrcid)
                ))));
        }

        // Searching with type of funding id requires exact match.
        if (!string.IsNullOrWhiteSpace(parameters.TypeOfFunding))
        {
            subQueries.Add(t => t.Nested(query => query
                .Path(p => p.TypeOfFunding)
                .Query(q => q.Term(term => term
                    .Field(f => f.TypeOfFunding!.TypeId)
                    .Value(parameters.TypeOfFunding)
                ))));
        }

        return subQueries;
    }

    private static IEnumerable<Func<QueryContainerDescriptor<FundingDecision>, QueryContainer>> CreateFilters(
        FundingDecisionSearchParameters parameters)
    {
        var filters = new List<Func<QueryContainerDescriptor<FundingDecision>, QueryContainer>>();

        // Add date filter for FundingStartYearFrom
        filters.Add(x => x
            .Bool(b => b
                .Should(s =>
                    s.Range(r => r
                        .Field(f => f.FundingStartYear)
                        .GreaterThanOrEquals(parameters.FundingStartYearFrom ?? 0)
                        .LessThan(2100))
                    ||
                    !s.Exists(q => q.Field(f => f.FundingStartYear))
                )));

        return filters;
    }
}