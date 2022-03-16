﻿using Api.Models.FundingCall;
using Nest;

namespace Api.Services.ElasticSearchQueryGenerators
{
    /// <summary>
    /// Service responsible for generating a search query for FundingCalls from ElasticSearch.
    /// </summary>
    public class FundingCallQueryGenerator : QueryGeneratorBase<FundingCallSearchParameters, FundingCall>
    {
        public FundingCallQueryGenerator(IConfiguration configuration) : base(configuration)
        {
        }

        protected override Func<SearchDescriptor<FundingCall>, ISearchRequest> GenerateQueryForIndex(FundingCallSearchParameters parameters, string indexName)
        {
            var subQueries = new List<Func<QueryContainerDescriptor<FundingCall>, QueryContainer>>();

            // When searching with Name, search from Fi,Sv,En names.
            if (!string.IsNullOrWhiteSpace(parameters.Name))
            {
                subQueries.Add(t => t.MultiMatch(query => query
                    .Type(TextQueryType.PhrasePrefix)
                    .Fields("nameFi, nameSv, nameEn")
                    .Query(parameters.Name)));
            }

            // When searching with FoundationName, search from Fi,Sv,En names.
            if (!string.IsNullOrWhiteSpace(parameters.FoundationName))
            {
                subQueries.Add(t => t.Nested(query => query
                    .Path("foundation")
                    .Query(q => q.MultiMatch(query => query
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields("foundation.nameFi, foundation.nameSv, foundation.nameEn")
                        .Query(parameters.FoundationName)
                    ))));
            }

            // Searching with business id requires exact match.
            if (!string.IsNullOrWhiteSpace(parameters.FoundationBusinessId))
            {
                subQueries.Add(t => t.Nested(query => query
                    .Path("foundation")
                    .Query(q => q.Term(term => term
                        .Field("foundation.businessId")
                        .Value(parameters.FoundationBusinessId)
                    ))));
            }

            return searchDescriptor => searchDescriptor
                .Index(indexName)
                .Query(queryDescriptor => queryDescriptor
                    .Bool(boolDescriptor => boolDescriptor
                        .Must(subQueries)));
        }
    }
}