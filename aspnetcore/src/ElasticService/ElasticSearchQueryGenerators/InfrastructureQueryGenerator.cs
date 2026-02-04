using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models.Infrastructure;
using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

/// <summary>
/// Service responsible for generating a search query for Infrastructures from ElasticSearch.
/// </summary>
public class InfrastructureQueryGenerator : QueryGeneratorBase<InfrastructureSearchParameters, Infrastructure>
{
    public InfrastructureQueryGenerator(IndexNameSettings configuration) : base(configuration)
    {
    }

    protected override Func<QueryContainerDescriptor<Infrastructure>, QueryContainer> GenerateQueryForSearch(InfrastructureSearchParameters parameters)
    {
        var subQueries = CreateSubQueries(parameters);
        var filters = CreateFilters(parameters);

        return queryDescriptor => queryDescriptor
            .Bool(boolDescriptor => boolDescriptor
                .Must(subQueries)
                .Filter(filters)
            );
    }

    private static IEnumerable<Func<QueryContainerDescriptor<Infrastructure>, QueryContainer>> CreateSubQueries(InfrastructureSearchParameters parameters)
    {
        var subQueries = new List<Func<QueryContainerDescriptor<Infrastructure>, QueryContainer>>();

        // PersistentIdentifierUrn
        if (!string.IsNullOrWhiteSpace(parameters.PersistentIdentifierUrn))
        {
            subQueries.Add(t =>
                t.MatchPhrase(query => query.Field(f => f.InfraIdentifier!.PersistentIdentifierURN)
                    .Query(parameters.PersistentIdentifierUrn)));
        }

        // OtherPersistentIdentifier
        if (!string.IsNullOrWhiteSpace(parameters.OtherPersistentIdentifier))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.InfraIdentifier!.OtherPid)
                    .Query(q => q
                        .MatchPhrase(m => m
                            .Field(f => f.InfraIdentifier!.OtherPid!.First().Pid)
                            .Query(parameters.OtherPersistentIdentifier)))));
        }

        // LocalIdentifier
        if (!string.IsNullOrWhiteSpace(parameters.LocalIdentifier))
        {
            subQueries.Add(t =>
                t.MatchPhrase(query => query.Field(f => f.InfraIdentifier!.LocalIdentifier)
                    .Query(parameters.LocalIdentifier)));
        }

        // InfraName
        if (!string.IsNullOrWhiteSpace(parameters.InfraName))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.InfraName)
                    .Query(q => q
                        .MatchPhrase(m => m
                            .Field(f => f.InfraName!.First().TextContent)
                            .Query(parameters.InfraName)))));
        }

        // InfraDescription
        if (!string.IsNullOrWhiteSpace(parameters.InfraDescription))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.InfraDescription)
                    .Query(q => q
                        .MatchPhrase(m => m
                            .Field(f => f.InfraDescription!.First().TextContent)
                            .Query(parameters.InfraDescription)))));
        }

        // Acronym
        if (!string.IsNullOrWhiteSpace(parameters.Acronym))
        {
            subQueries.Add(t =>
                t.MatchPhrase(query => query.Field(f => f.Acronym)
                    .Query(parameters.Acronym)));
        }

        // Esfri
        if (!string.IsNullOrWhiteSpace(parameters.Esfri))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.Esfri)
                    .Query(q => q
                        .Term(m => m
                            .Field(f => f.Esfri!.First().CodeValue)
                            .Value(parameters.Esfri)))));
        }

        // FinlandRoadmapInfrastructure
        if (parameters.FinlandRoadmapInfrastructure.HasValue)
        {
            subQueries.Add(t =>
                t.Term(term => term
                    .Field(f => f.FinlandRoadmapInfrastructure)
                    .Value(parameters.FinlandRoadmapInfrastructure.Value)));
        }

        // ResponsibleOrganizationRor
        if (!string.IsNullOrWhiteSpace(parameters.ResponsibleOrganizationRor))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.ResponsibleOrganization)
                    .Query(q => q
                        .Term(m => m
                            .Field(f => f.ResponsibleOrganization!.OrganizationIdentifierROR)
                            .Value(parameters.ResponsibleOrganizationRor)))));
        }
        
        // ResponsibleOrganizationBusinessId
        if (!string.IsNullOrWhiteSpace(parameters.ResponsibleOrganizationBusinessId))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.ResponsibleOrganization)
                    .Query(q => q
                        .Term(m => m
                            .Field(f => f.ResponsibleOrganization!.OrganizationIdentifier)
                            .Value(parameters.ResponsibleOrganizationBusinessId)))));
        }

        // OrganizationParticipatesInfrastructureRor
        if (!string.IsNullOrWhiteSpace(parameters.OrganizationParticipatesInfrastructureRor))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.OrganizationParticipatesInfrastructure)
                    .Query(q => q
                        .Term(m => m
                            .Field(f => f.OrganizationParticipatesInfrastructure!.First().OrganizationIdentifierROR)
                            .Value(parameters.OrganizationParticipatesInfrastructureRor)))));
        }

        // OrganizationParticipatesInfrastructureBusinessId
        if (!string.IsNullOrWhiteSpace(parameters.OrganizationParticipatesInfrastructureBusinessId))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.OrganizationParticipatesInfrastructure)
                    .Query(q => q
                        .Term(m => m
                            .Field(f => f.OrganizationParticipatesInfrastructure!.First().OrganizationIdentifier)
                            .Value(parameters.OrganizationParticipatesInfrastructureBusinessId)))));
        }

        // FieldOfScience
        if (!string.IsNullOrWhiteSpace(parameters.FieldOfScience))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.FieldOfScience)
                    .Query(q => q
                        .Term(m => m
                            .Field(f => f.FieldOfScience!.First().CodeValue)
                            .Value(parameters.FieldOfScience)))));
        }

        // InfraStartsOnYear
        if (parameters.InfraStartsOnYear.HasValue)
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.InfraStartsOn)
                    .Query(q => q
                        .Term(m => m
                            .Field(f => f.InfraStartsOn!.Year)
                            .Value(parameters.InfraStartsOnYear.Value)))));
        }

        // InfraEndsOnYear
        if (parameters.InfraEndsOnYear.HasValue)
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.InfraEndsOn)
                    .Query(q => q
                        .Term(m => m
                            .Field(f => f.InfraEndsOn!.Year)
                            .Value(parameters.InfraEndsOnYear.Value)))));
        }

        // VisitingAddressCountryCode
        if (!string.IsNullOrWhiteSpace(parameters.VisitingAddressCountryCode))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.InfraContactInformation)
                    .Query(q => q
                        .Bool(b => b
                            .Must(mu => mu
                                .Term(m => m
                                    .Field(f => f.InfraContactInformation!.First().VisitingAddress!.Country!.CodeValue)
                                    .Value(parameters.VisitingAddressCountryCode)))))));
        }

        return subQueries;
    }

    private static IEnumerable<Func<QueryContainerDescriptor<Infrastructure>, QueryContainer>> CreateFilters(InfrastructureSearchParameters parameters)
    {
        var filters = new List<Func<QueryContainerDescriptor<Infrastructure>, QueryContainer>>();

        return filters;
    }

    protected override Func<QueryContainerDescriptor<Infrastructure>, QueryContainer> GenerateQueryForSingle(string id)
    {
        return queryContainerDescriptor => queryContainerDescriptor.Term(query => query.Field(f => f.InfraIdentifier.PersistentIdentifierURN).Value(id));
    }

    protected override Func<SortDescriptor<Infrastructure>, IPromise<IList<ISort>>> GenerateSortForSearch(InfrastructureSearchParameters parameters)
    {
        // Sort infrastructures
        return sortDescriptor => sortDescriptor
            .Field(f => f.ExportSortId, SortOrder.Ascending); // DO NOT REMOVE, prevents duplicates in the result set
    }
}