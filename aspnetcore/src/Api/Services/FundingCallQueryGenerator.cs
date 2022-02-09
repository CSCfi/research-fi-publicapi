using Api.Models;
using Nest;

namespace Api.Services
{
    public class FundingCallQueryGenerator : IQueryGenerator<FundingCall>
    {
        private readonly string _indexName = "funding-call";

        public Func<SearchDescriptor<FundingCall>, ISearchRequest> GenerateQuery(string searchText)
        {
            return t => t
                .Index(_indexName)
                .Query(q => q
                    .MultiMatch(query => query
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields("nameFi")
                        .Query(searchText)));
        }

    }
}
