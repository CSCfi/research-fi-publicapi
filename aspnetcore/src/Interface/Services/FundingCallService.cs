﻿using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using FundingCall = CSC.PublicApi.Interface.Models.FundingCall.FundingCall;

namespace CSC.PublicApi.Interface.Services;

public class FundingCallService : IFundingCallService
{
    private readonly ILogger<FundingCallService> _logger;
    private readonly IMapper _mapper;
    private readonly ISearchService<FundingCallSearchParameters, Service.Models.FundingCall.FundingCall> _searchService;

    public FundingCallService(ILogger<FundingCallService> logger,
        IMapper mapper,
        ISearchService<FundingCallSearchParameters, Service.Models.FundingCall.FundingCall> searchService)
    {
        _logger = logger;
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task<(IEnumerable<FundingCall>, SearchResult)> GetFundingCalls(GetFundingCallQueryParameters queryParameters)
    {
        var searchParameters = _mapper.Map<FundingCallSearchParameters>(queryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, queryParameters.PageNumber, queryParameters.PageSize);

        return (_mapper.Map<IEnumerable<FundingCall>>(result), searchResult);
    }

    public async Task PostFundCall(FundingCall fundingCall)
    {
        // TODO: only NameFi mapped to entity. Not using final models yet.
        /*await _unitOfWork
            .FundingCalls
            .AddAsync(new DimCallProgramme
            {
                NameFi = fundingCall.NameFi,
                SourceId = "api",
                DimRegisteredDataSourceId = -1
            });
        await _unitOfWork.CompleteAsync();*/
    }
}