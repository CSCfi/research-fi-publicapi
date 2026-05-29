using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models.Infrastructure;
using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

/// <summary>
/// Service responsible for generating a search query for Infrastructure services from ElasticSearch.
/// </summary>
public class InfrastructureServiceQueryGenerator : QueryGeneratorBase<InfrastructureServiceSearchParameters, CSC.PublicApi.Service.Models.Infrastructure.Service>
{
    public InfrastructureServiceQueryGenerator(IndexNameSettings configuration) : base(configuration)
    {
    }

    protected override Func<QueryContainerDescriptor<CSC.PublicApi.Service.Models.Infrastructure.Service>, QueryContainer> GenerateQueryForSearch(InfrastructureServiceSearchParameters parameters)
    {
        var subQueries = CreateSubQueries(parameters);
        var filters = CreateFilters(parameters);

        return queryDescriptor => queryDescriptor
            .Bool(boolDescriptor => boolDescriptor
                .Must(subQueries)
                .Filter(filters)
            );
    }

    private static IEnumerable<Func<QueryContainerDescriptor<CSC.PublicApi.Service.Models.Infrastructure.Service>, QueryContainer>> CreateSubQueries(InfrastructureServiceSearchParameters parameters)
    {
        var subQueries = new List<Func<QueryContainerDescriptor<CSC.PublicApi.Service.Models.Infrastructure.Service>, QueryContainer>>();

        // KeyIdentifierUrn
        if (!string.IsNullOrWhiteSpace(parameters.KeyIdentifierUrn))
        {
            subQueries.Add(t =>
                t.MatchPhrase(query => query.Field(f => f.ServiceIdentifier.KeyIdentifierURN)
                    .Query(parameters.KeyIdentifierUrn)));
        }

        // OtherPersistentIdentifier
        if (!string.IsNullOrWhiteSpace(parameters.OtherPersistentIdentifier))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.ServiceIdentifier.OtherPid)
                    .Query(q => q
                        .MatchPhrase(m => m
                            .Field(f => f.ServiceIdentifier.OtherPid.First().PidContent)
                            .Query(parameters.OtherPersistentIdentifier)))));
        }

        // LocalIdentifier
        if (!string.IsNullOrWhiteSpace(parameters.LocalIdentifier))
        {
            subQueries.Add(t =>
                t.MatchPhrase(query => query.Field(f => f.ServiceIdentifier!.LocalIdentifier)
                    .Query(parameters.LocalIdentifier)));
        }

        // ServiceName
        if (!string.IsNullOrWhiteSpace(parameters.ServiceName))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.ServiceName)
                    .Query(q => q
                        .MatchPhrase(m => m
                            .Field(f => f.ServiceName.First().TextContent)
                            .Query(parameters.ServiceName)))));
        }

        // ServiceDescription
        if (!string.IsNullOrWhiteSpace(parameters.ServiceDescription))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.ServiceDescription)
                    .Query(q => q
                        .MatchPhrase(m => m
                            .Field(f => f.ServiceDescription.First().TextContent)
                            .Query(parameters.ServiceDescription)))));
        }

        // IsPartOfInfrastructureURN
        if (!string.IsNullOrWhiteSpace(parameters.IsPartOfInfrastructureURN))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.IsPartOfInfrastructure)
                    .Query(q => q
                        .MatchPhrase(m => m
                            .Field(f => f.IsPartOfInfrastructure.InfraIdentifier.KeyIdentifierURN)
                            .Query(parameters.IsPartOfInfrastructureURN)))));
        }

        // IsPartOfInfrastructureResponsibleOrganizationId
        if (!string.IsNullOrWhiteSpace(parameters.IsPartOfInfrastructureResponsibleOrganizationId))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.IsPartOfInfrastructure)
                    .Query(q => q
                        .Nested(n2 => n2
                            .Path(p => p.IsPartOfInfrastructure!.ResponsibleOrganization!.OrganizationIdentifier)
                            .Query(q2 => q2
                                .Term(m => m
                                    .Field(f => f.IsPartOfInfrastructure!.ResponsibleOrganization!.OrganizationIdentifier!.First().PidContent)
                                    .Value(parameters.IsPartOfInfrastructureResponsibleOrganizationId)))))));
        }

        // IsPartOfInfrastructureInfraParticipatingOrganizationsId
        if (!string.IsNullOrWhiteSpace(parameters.IsPartOfInfrastructureInfraParticipatingOrganizationsId))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.IsPartOfInfrastructure)
                    .Query(q => q
                        .Nested(n2 => n2
                            .Path(p => p.IsPartOfInfrastructure!.InfraParticipatingOrganizations!.First().OrganizationIdentifier)
                            .Query(q2 => q2
                                .Term(m => m
                                    .Field(f => f.IsPartOfInfrastructure!.InfraParticipatingOrganizations!.First().OrganizationIdentifier!.First().PidContent)
                                    .Value(parameters.IsPartOfInfrastructureInfraParticipatingOrganizationsId)))))));
        }

        // ServiceStartsOnYear
        if (parameters.ServiceStartsOnYear.HasValue)
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.ServiceStartsOn)
                    .Query(q => q
                        .Term(m => m
                            .Field(f => f.ServiceStartsOn!.Year)
                            .Value(parameters.ServiceStartsOnYear.Value)))));
        }

        // ServiceEndsOnYear
        if (parameters.ServiceEndsOnYear.HasValue)
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.ServiceEndsOn)
                    .Query(q => q
                        .Term(m => m
                            .Field(f => f.ServiceEndsOn!.Year)
                            .Value(parameters.ServiceEndsOnYear.Value)))));
        }

        // ServiceEndsByYear
        if (parameters.ServiceEndsByYear.HasValue)
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.ServiceEndsOn)
                    .Query(q => q
                        .Range(r => r
                            .Field(f => f.ServiceEndsOn!.Year)
                            .LessThanOrEquals(parameters.ServiceEndsByYear.Value)))));
        }

        return subQueries;
    }

    private static IEnumerable<Func<QueryContainerDescriptor<CSC.PublicApi.Service.Models.Infrastructure.Service>, QueryContainer>> CreateFilters(InfrastructureServiceSearchParameters parameters)
    {
        var filters = new List<Func<QueryContainerDescriptor<CSC.PublicApi.Service.Models.Infrastructure.Service>, QueryContainer>>();

        return filters;
    }

    protected override Func<QueryContainerDescriptor<CSC.PublicApi.Service.Models.Infrastructure.Service>, QueryContainer> GenerateQueryForSingle(string id)
    {
        return queryContainerDescriptor => queryContainerDescriptor.Term(query => query.Field(f => f.ServiceIdentifier.KeyIdentifierURN).Value(id));
    }

    protected override Func<SortDescriptor<CSC.PublicApi.Service.Models.Infrastructure.Service>, IPromise<IList<ISort>>> GenerateSortForSearch(InfrastructureServiceSearchParameters parameters)
    {
        // Sort infrastructures
        return sortDescriptor => sortDescriptor
            .Field(f => f.ExportSortId, SortOrder.Ascending); // DO NOT REMOVE, prevents duplicates in the result set
    }
}