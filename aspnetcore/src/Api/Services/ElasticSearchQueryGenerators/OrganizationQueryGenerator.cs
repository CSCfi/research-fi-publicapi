using Api.Configuration;
using Api.Models.Organization;
using Nest;

namespace Api.Services.ElasticSearchQueryGenerators
{
    /// <summary>
    /// Service responsible for generating a search query for Organizations from ElasticSearch.
    /// </summary>
    public class OrganizationQueryGenerator : QueryGeneratorBase<OrganizationSearchParameters, Organization>
    {
        public OrganizationQueryGenerator(IndexNameSettings configuration) : base(configuration)
        {
        }

        protected override Func<SearchDescriptor<Organization>, ISearchRequest> GenerateQueryForIndex(OrganizationSearchParameters parameters, string indexName)
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

            return searchDescriptor => searchDescriptor
                .Index(indexName)
                .Query(queryDescriptor => queryDescriptor
                    .Bool(boolDescriptor => boolDescriptor
                        .Must(subQueries)
                        .Filter(filters)
                        )
                );
        }
    }
}
