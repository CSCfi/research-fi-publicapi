using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.FundingDecision;
using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

public class FundingDecisionQueryGenerator : QueryGeneratorBase<FundingDecisionSearchParameters, FundingDecision>
{
    public FundingDecisionQueryGenerator(IndexNameSettings configuration) : base(configuration)
    {
    }

    protected override Func<QueryContainerDescriptor<FundingDecision>, QueryContainer> GenerateQueryForSearch(
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

    protected override Func<QueryContainerDescriptor<FundingDecision>, QueryContainer> GenerateQueryForSingle(string id)
    {
        throw new NotImplementedException();
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
            subQueries.Add(t => t.MultiMatch(multi => multi
                .Type(TextQueryType.PhrasePrefix)
                .Fields(r => r
                    .Field(f => f.Funder.Suffix(nameof(Funder.NameFi)))
                    .Field(f => f.Funder.Suffix(nameof(Funder.NameSv)))
                    .Field(f => f.Funder.Suffix(nameof(Funder.NameEn))))
                .Query(parameters.FunderName)
            ));
        }

        // When searching with FundedOrganisation, search from organization consortia Fi,Sv,En names.
        if (!string.IsNullOrWhiteSpace(parameters.FundedOrganization))
        {
            subQueries.Add(t => t.MultiMatch(multi => multi
                .Type(TextQueryType.PhrasePrefix)
                .Fields(r => r
                    .Field(f => f.FundingReceivers.Suffix(nameof(Organization)).Suffix(nameof(Organization.NameFi)))
                    .Field(f => f.FundingReceivers.Suffix(nameof(Organization)).Suffix(nameof(Organization.NameSv)))
                    .Field(f => f.FundingReceivers.Suffix(nameof(Organization)).Suffix(nameof(Organization.NameEn))))
                .Query(parameters.FundedOrganization)
            ));
        }

        // Searching with funded person first name
        if (!string.IsNullOrWhiteSpace(parameters.FundedPersonFirstName))
        {
            subQueries.Add(t => t.QueryString(multi => multi
                .Type(TextQueryType.PhrasePrefix)
                .Fields(fields =>
                    fields.Field(f => f.FundingReceivers.Suffix(nameof(FundingReceiver.Person)).Suffix(nameof(Person.FirstNames))))
                .Query(parameters.FundedPersonFirstName)
            ));
        }

        // Searching with funded person last name
        if (!string.IsNullOrWhiteSpace(parameters.FundedPersonLastName))
        {
            subQueries.Add(t => t.QueryString(multi => multi
                .Type(TextQueryType.PhrasePrefix)
                .Fields(fields =>
                    fields.Field(f => f.FundingReceivers.Suffix(nameof(FundingReceiver.Person)).Suffix(nameof(Person.LastName))))
                .Query(parameters.FundedPersonLastName)
            ));
        }

        return subQueries;
    }

    private static IEnumerable<Func<QueryContainerDescriptor<FundingDecision>, QueryContainer>> CreateFilters(
        FundingDecisionSearchParameters parameters)
    {
        var filters = new List<Func<QueryContainerDescriptor<FundingDecision>, QueryContainer>>();
        
        // Searching with project number requires exact match.
        if (!string.IsNullOrWhiteSpace(parameters.FunderProjectNumber))
        {
            filters.Add(t => t.Term(fd => fd.FunderProjectNumber, parameters.FunderProjectNumber));
        }
        
        // Searching with funder id requires exact match.
        if (!string.IsNullOrWhiteSpace(parameters.FunderId))
        {
            filters.Add(t => t.Term(term => term
                .Field(f => f.Funder!.Pids.Suffix(nameof(PersistentIdentifier.Content)))
                .Value(parameters.FunderId)
            ));
        }
        
        // Searching with funded organization id requires exact match.
        if (!string.IsNullOrWhiteSpace(parameters.FundedPersonOrcid))
        {
            filters.Add(t => t.Term(term => term
                .Field(f => f.FundingReceivers.Suffix(nameof(FundingReceiver.Person)).Suffix(nameof(Person.OrcId)))
                .Value(parameters.FundedPersonOrcid)
            ));
        }
        
        // Searching with funded organization id requires exact match.
        if (!string.IsNullOrWhiteSpace(parameters.FundedOrganizationId))
        {
            filters.Add(t => t.Term(term => term
                .Field(f => f.FundingReceivers.Suffix(nameof(Organization)).Suffix(nameof(Organization.Pids))
                    .Suffix(nameof(PersistentIdentifier.Content)))
                .Value(parameters.FundedOrganizationId)
            ));
        }

        // Searching with type of funding id requires exact match.
        if (!string.IsNullOrWhiteSpace(parameters.TypeOfFunding))
        {
            filters.Add(t => t.Term(term => term
                .Field(f => f.TypeOfFunding!.Code)
                .Value(parameters.TypeOfFunding)
            ));
        }

        // Add date filter for FundingStartYearFrom
        if (parameters.FundingStartYearFrom is not null)
        {
            filters.Add(x => x
                .Bool(b => b
                    .Should(s => (
                        s.DateRange(r => r
                            .Field(f => f.FundingStartDate)
                            .GreaterThanOrEquals(parameters.FundingStartYearFrom))
                        ||
                        !s.Exists(q => q.Field(f => f.FundingStartDate)))
                    )));    
        }
        

        return filters;
    }

    protected override Func<SortDescriptor<FundingDecision>, IPromise<IList<ISort>>> GenerateSortForSearch(FundingDecisionSearchParameters parameters)
    {
        // Sort funding decisions
        return sortDescriptor => sortDescriptor
            .Field(f => f.FundingStartDate, SortOrder.Descending)
            .Field(f => f.ExportSortId, SortOrder.Ascending); // DO NOT REMOVE, prevents duplicates in the result set
    }
}