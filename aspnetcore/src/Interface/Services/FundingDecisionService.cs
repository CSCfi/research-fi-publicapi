﻿using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.FundingDecision;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services;

public class FundingDecisionService : IFundingDecisionService
{
    private readonly ILogger<FundingDecisionService> _logger;
    private readonly IMapper _mapper;
    private readonly ISearchService<FundingDecisionSearchParameters, Service.Models.FundingDecision.FundingDecision> _searchService;

    public FundingDecisionService(ILogger<FundingDecisionService> logger, IMapper mapper, ISearchService<FundingDecisionSearchParameters, Service.Models.FundingDecision.FundingDecision> searchService)
    {
        _logger = logger;
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task<(IEnumerable<FundingDecision>, SearchResult)> GetFundingDecisions(GetFundingDecisionQueryParameters queryParameters)
    {
        var searchParameters = _mapper.Map<FundingDecisionSearchParameters>(queryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, queryParameters.PageNumber, queryParameters.PageSize);

        return (_mapper.Map<IEnumerable<FundingDecision>>(result), searchResult);
    }
}