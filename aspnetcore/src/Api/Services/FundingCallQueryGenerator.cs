using Api.Models.FundingCall;
using Nest;

namespace Api.Services
{
    /// <summary>
    /// Service responsible for generating a search query for FundingCalls from ElasticSearch.
    /// </summary>
    public class FundingCallQueryGenerator : IQueryGenerator<FundingCallSearchParameters, FundingCall>
    {
        private readonly string _indexName;
        public FundingCallQueryGenerator(IConfiguration configuration)
        {
            _indexName = configuration["IndexNames:FundingCall"] ?? throw new InvalidOperationException("FundingCall index config missing.");
        }

        public Func<SearchDescriptor<FundingCall>, ISearchRequest> GenerateQuery(FundingCallSearchParameters parameters)
        {
            var subQueries = new List<Func<QueryContainerDescriptor<FundingCall>, QueryContainer>>();

            // When searching with Name, search from Fi,Sv,En names.
            if(!string.IsNullOrWhiteSpace(parameters.Name))
            {
                subQueries.Add(t => t.MultiMatch(query => query
                    .Type(TextQueryType.PhrasePrefix)
                    .Fields("nameFi, nameSv, nameEn")
                    .Query(parameters.Name)));
            }

            // When searching with FoundationName, search from Fi,Sv,En names.
            if (!string.IsNullOrWhiteSpace(parameters.FoundationName))
            {
                subQueries.Add(t => t.MultiMatch(query => query
                    .Type(TextQueryType.PhrasePrefix)
                    .Fields("foundation.nameFi, foundation.nameSv, foundation.nameEn")
                    .Query(parameters.FoundationName)));
            }

            // Searching with business id requires exact match.
            if (!string.IsNullOrWhiteSpace(parameters.FoundationBusinessId))
            {
                subQueries.Add(t => t.Term(t => t
                    .Field("foundation.businessId.keyword")
                    .Value(parameters.FoundationBusinessId)));
            }

            return searchDescriptor => searchDescriptor
                .Index(_indexName)
                .Query(queryDescriptor => queryDescriptor
                    .Bool(boolDescriptor => boolDescriptor
                        .Must(subQueries)));
                
        }

    }
}
