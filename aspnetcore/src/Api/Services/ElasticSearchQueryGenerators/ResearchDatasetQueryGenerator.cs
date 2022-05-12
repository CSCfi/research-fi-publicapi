using Api.Configuration;
using Api.Models.ResearchDataset;
using Nest;

namespace Api.Services.ElasticSearchQueryGenerators
{
    public class ResearchDatasetQueryGenerator : QueryGeneratorBase<ResearchDatasetSearchParameters, ResearchDataset>
    {
        public ResearchDatasetQueryGenerator(IndexNameSettings configuration) : base(configuration)
        {
        }

        protected override Func<SearchDescriptor<ResearchDataset>, ISearchRequest> GenerateQueryForIndex(ResearchDatasetSearchParameters parameters, string indexName)
        {
            var subQueries = new List<Func<QueryContainerDescriptor<ResearchDataset>, QueryContainer>>();
            var filters = new List<Func<QueryContainerDescriptor<ResearchDataset>, QueryContainer>>();

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
