using Api.Configuration;
using Api.Models.FundingCall;
using Nest;

namespace Api.Services.ElasticSearchQueryGenerators
{
    /// <summary>
    /// Service responsible for generating a search query for FundingCalls from ElasticSearch.
    /// </summary>
    public class FundingCallQueryGenerator : QueryGeneratorBase<FundingCallSearchParameters, FundingCall>
    {
        public FundingCallQueryGenerator(IndexNameSettings configuration) : base(configuration)
        {
        }

        protected override Func<QueryContainerDescriptor<FundingCall>, QueryContainer> GenerateQueryForIndex(FundingCallSearchParameters parameters)
        {
            var subQueries = new List<Func<QueryContainerDescriptor<FundingCall>, QueryContainer>>();
            var filters = new List<Func<QueryContainerDescriptor<FundingCall>, QueryContainer>>();

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

            // Searching with category code requires exact match.
            if (!string.IsNullOrWhiteSpace(parameters.CategoryCode))
            {
                subQueries.Add(t => t.Nested(query => query
                    .Path("categories")
                    .Query(q => q.Term(term => term
                        .Field("categories.codeValue")
                        .Value(parameters.CategoryCode)
                    ))));
            }

            // Add date filters for OpenDate & DueDate
            filters.Add(x => x
                .Bool(b => b
                    .Should(s =>
                        (
                        s.DateRange(r => r
                            .Field("callProgrammeOpenDate")
                            .LessThanOrEquals(parameters.DateTo ?? DateTime.MaxValue))
                        ||
                        !s.Exists(b => b.Field("callProgrammeOpenDate"))
                        )

                        &&
                        (
                        s.DateRange(r => r
                            .Field("callProgrammeDueDate")
                            .GreaterThanOrEquals(parameters.DateFrom ?? DateTime.MinValue))
                        ||
                        !s.Exists(b => b.Field("callProgrammeDueDate"))
                        )

                    )));

            return queryDescriptor => queryDescriptor
                    .Bool(boolDescriptor => boolDescriptor
                        .Must(subQueries)
                        .Filter(filters)
                        );
        }
    }
}
