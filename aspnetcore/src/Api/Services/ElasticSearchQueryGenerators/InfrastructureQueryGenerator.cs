using Api.Models.Infrastructure;
using Nest;

namespace Api.Services.ElasticSearchQueryGenerators
{
    /// <summary>
    /// Service responsible for generating a search query for Infrastructures from ElasticSearch.
    /// </summary>
    public class InfrastructureQueryGenerator : QueryGeneratorBase<InfrastructureSearchParameters, Infrastructure>
    {
        public InfrastructureQueryGenerator(IConfiguration configuration) : base(configuration)
        {
        }

        protected override Func<SearchDescriptor<Infrastructure>, ISearchRequest> GenerateQueryForIndex(InfrastructureSearchParameters parameters, string indexName)
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
