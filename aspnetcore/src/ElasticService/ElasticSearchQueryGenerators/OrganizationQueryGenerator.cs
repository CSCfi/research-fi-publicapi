using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models.Organization;
using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

/// <summary>
/// Service responsible for generating a search query for Organizations from ElasticSearch.
/// </summary>
public class OrganizationQueryGenerator : QueryGeneratorBase<OrganizationSearchParameters, Organization>
{
    public OrganizationQueryGenerator(IndexNameSettings configuration) : base(configuration)
    {
    }

    protected override Func<QueryContainerDescriptor<Organization>, QueryContainer> GenerateQueryForSearch(OrganizationSearchParameters parameters)
    {
        var subQueries = new List<Func<QueryContainerDescriptor<Organization>, QueryContainer>>();
        var filters = new List<Func<QueryContainerDescriptor<Organization>, QueryContainer>>();

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

    protected override Func<QueryContainerDescriptor<Organization>, QueryContainer> GenerateQueryForSingle(string id)
    {
        throw new NotImplementedException();
    }

    protected override Func<SortDescriptor<Organization>, IPromise<IList<ISort>>> GenerateSortForSearch(OrganizationSearchParameters parameters)
    {
        // Sort organizations
        return sortDescriptor => sortDescriptor
            .Field(f => f.NameFi, SortOrder.Ascending);
    }
}