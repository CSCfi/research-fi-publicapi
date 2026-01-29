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

        // PersistentIdentifierUrn
        if (!string.IsNullOrWhiteSpace(parameters.PersistentIdentifierUrn))
        {
            subQueries.Add(t =>
                t.MatchPhrase(query => query.Field(f => f.ServiceIdentifier.PersistentIdentifierURN)
                    .Query(parameters.PersistentIdentifierUrn)));
        }

        // OtherPersistentIdentifier
        if (!string.IsNullOrWhiteSpace(parameters.OtherPersistentIdentifier))
        {
            subQueries.Add(t =>
                t.Nested(n => n
                    .Path(p => p.ServiceIdentifier.OtherPid)
                    .Query(q => q
                        .MatchPhrase(m => m
                            .Field(f => f.ServiceIdentifier.OtherPid.First().Pid)
                            .Query(parameters.OtherPersistentIdentifier)))));
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

        return subQueries;
    }

    private static IEnumerable<Func<QueryContainerDescriptor<CSC.PublicApi.Service.Models.Infrastructure.Service>, QueryContainer>> CreateFilters(InfrastructureServiceSearchParameters parameters)
    {
        var filters = new List<Func<QueryContainerDescriptor<CSC.PublicApi.Service.Models.Infrastructure.Service>, QueryContainer>>();

        return filters;
    }

    protected override Func<QueryContainerDescriptor<CSC.PublicApi.Service.Models.Infrastructure.Service>, QueryContainer> GenerateQueryForSingle(string id)
    {
        return queryContainerDescriptor => queryContainerDescriptor.Term(query => query.Field(f => f.ServiceIdentifier.PersistentIdentifierURN).Value(id));
    }

    protected override Func<SortDescriptor<CSC.PublicApi.Service.Models.Infrastructure.Service>, IPromise<IList<ISort>>> GenerateSortForSearch(InfrastructureServiceSearchParameters parameters)
    {
        // Sort infrastructures
        return sortDescriptor => sortDescriptor
            .Field(f => f.ExportSortId, SortOrder.Ascending); // DO NOT REMOVE, prevents duplicates in the result set
    }
}