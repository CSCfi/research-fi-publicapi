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
        var subQueries = new List<Func<QueryContainerDescriptor<Infrastructure>, QueryContainer>>();
        var filters = new List<Func<QueryContainerDescriptor<Infrastructure>, QueryContainer>>();

        // When searching with Name, search from Fi,Sv,En names.
        if (!string.IsNullOrWhiteSpace(parameters.Name))
        {
            subQueries.Add(t => t.MultiMatch(query => query
                .Type(TextQueryType.PhrasePrefix)
                .Fields("nameFi, nameSv, nameEn")
                .Query(parameters.Name)));
        }

        return queryDescriptor => queryDescriptor
            .Bool(boolDescriptor => boolDescriptor
                .Must(subQueries)
                .Filter(filters)
            );
    }

    protected override Func<QueryContainerDescriptor<Infrastructure>, QueryContainer> GenerateQueryForSingle(string id)
    {
        throw new NotImplementedException();
    }

    protected override Func<SortDescriptor<Infrastructure>, IPromise<IList<ISort>>> GenerateSortForSearch(InfrastructureSearchParameters parameters)
    {
        // Sort infrastructures
        return sortDescriptor => sortDescriptor
            .Field(f => f.NameFi, SortOrder.Ascending);
    }
}