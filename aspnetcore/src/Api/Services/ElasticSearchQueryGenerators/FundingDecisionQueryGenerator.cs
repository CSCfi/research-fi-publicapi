using Api.Models.FundingDecision;
using Nest;

namespace Api.Services.ElasticSearchQueryGenerators
{
    public class FundingDecisionQueryGenerator : QueryGeneratorBase<FundingDecisionSearchParameters, FundingDecision>
    {
        public FundingDecisionQueryGenerator(IConfiguration configuration) : base(configuration)
        {
        }

        protected override Func<SearchDescriptor<FundingDecision>, ISearchRequest> GenerateQueryForIndex(FundingDecisionSearchParameters searchParameters, string indexName)
        {
            var subQueries = new List<Func<QueryContainerDescriptor<FundingDecision>, QueryContainer>>();

            // When searching with Name, search from Fi,Sv,En names.
            if (!string.IsNullOrWhiteSpace(searchParameters.Name))
            {
                subQueries.Add(t => t.MultiMatch(query => query
                    .Type(TextQueryType.PhrasePrefix)
                    .Fields("projectNameFi, projectNameSv, projectNameEn")
                    .Query(searchParameters.Name)));
            }

            return searchDescriptor => searchDescriptor
                .Index(indexName)
                .Query(queryDescriptor => queryDescriptor
                    .Bool(boolDescriptor => boolDescriptor
                        .Must(subQueries)));
        }
    }
}
