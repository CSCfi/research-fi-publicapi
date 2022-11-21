using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models.FundingCall;
using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

/// <summary>
/// Service responsible for generating a search query for FundingCalls from ElasticSearch.
/// </summary>
public class FundingCallQueryGenerator : QueryGeneratorBase<FundingCallSearchParameters, FundingCall>
{
    public FundingCallQueryGenerator(IndexNameSettings configuration) : base(configuration)
    {
    }

    protected override Func<QueryContainerDescriptor<FundingCall>, QueryContainer> GenerateQueryForIndex(
        FundingCallSearchParameters parameters)
    {
        var subQueries = CreateSubQueries(parameters);
        var filters = CreateFilters(parameters);

        return queryDescriptor => queryDescriptor
            .Bool(boolDescriptor => boolDescriptor
                .Must(subQueries)
                .Filter(filters)
            );
    }

    private static IEnumerable<Func<QueryContainerDescriptor<FundingCall>, QueryContainer>> CreateSubQueries(
        FundingCallSearchParameters parameters)
    {
        var subQueries = new List<Func<QueryContainerDescriptor<FundingCall>, QueryContainer>>();

        // When searching with Name, search from Fi,Sv,En names.
        if (!string.IsNullOrWhiteSpace(parameters.Name))
        {
            subQueries.Add(t => t.MultiMatch(query => query
                .Type(TextQueryType.PhrasePrefix)
                .Fields(r => r
                    .Field(f => f.NameFi)
                    .Field(f => f.NameSv)
                    .Field(f => f.NameEn))
                .Query(parameters.Name)));
        }

        // When searching with FoundationName, search from Fi,Sv,En names.
        if (!string.IsNullOrWhiteSpace(parameters.FoundationName))
        {
            subQueries.Add(t => t.Nested(query => query
                .Path(f => f.Foundation)
                .Query(q => q.MultiMatch(multi => multi
                    .Type(TextQueryType.PhrasePrefix)
                    .Fields(r => r
                        .Field(f => f.Foundation.Suffix(nameof(Foundation.NameFi)))
                        .Field(f => f.Foundation.Suffix(nameof(Foundation.NameSv)))
                        .Field(f => f.Foundation.Suffix(nameof(Foundation.NameEn))))
                    .Query(parameters.FoundationName)
                ))));
        }

        return subQueries;
    }

    private static IEnumerable<Func<QueryContainerDescriptor<FundingCall>, QueryContainer>> CreateFilters(
        FundingCallSearchParameters parameters)
    {
        var filters = new List<Func<QueryContainerDescriptor<FundingCall>, QueryContainer>>();

        // Searching with business id requires exact match.
        if (!string.IsNullOrWhiteSpace(parameters.FoundationBusinessId))
        {
            filters.Add(t => t.Nested(query => query
                .Path(f => f.Foundation)
                .Query(q => q.Term(term => term
                    .Field(f => f.Foundation.Suffix(nameof(Foundation.BusinessId)))
                    .Value(parameters.FoundationBusinessId)
                ))));
        }

        // Searching with category code requires exact match.
        if (!string.IsNullOrWhiteSpace(parameters.CategoryCode))
        {
            filters.Add(t => t.Nested(query => query
                .Path(f => f.Categories)
                .Query(q => q.Term(term => term
                    .Field(f => f.Categories.Suffix(nameof(Category.CodeValue)))
                    .Value(parameters.CategoryCode)
                ))));
        }
        
        if (parameters.DateFrom != null || parameters.DateTo != null)
        {
            // Add date filters for OpenDate & DueDate
            filters.Add(x => x
                .Bool(b => b
                    .Should(s =>
                        (
                            s.DateRange(r => r
                                .Field(f => f.CallProgrammeOpenDate)
                                .LessThanOrEquals(parameters.DateTo ?? DateTime.MaxValue))
                            ||
                            !s.Exists(q => q.Field(f => f.CallProgrammeOpenDate))
                        )
                        &&
                        (
                            s.DateRange(r => r
                                .Field(f => f.CallProgrammeDueDate)
                                .GreaterThanOrEquals(parameters.DateFrom ?? DateTime.MinValue))
                            ||
                            !s.Exists(q => q.Field(f => f.CallProgrammeDueDate))
                        )
                    )));
        }

        return filters;
    }
}