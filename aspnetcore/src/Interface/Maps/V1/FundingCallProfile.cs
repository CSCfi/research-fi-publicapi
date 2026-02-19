/*
 * API version 1.0
 */
using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.CodeList;
using ResearchFi.FundingCall;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Maps.V1;

public class FundingCallProfile : Profile
{
    public FundingCallProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        
        CreateMap<GetFundingCallQueryParameters, FundingCallSearchParameters>();
        CreateMap<Service.Models.FundingCall.FundingCall, FundingCall>();
        CreateMap<Service.Models.FundingCall.Foundation, Foundation>();
        CreateMap<Service.Models.ReferenceData, Category>();
        CreateMap<Service.Models.ResearchfiUrl, ResearchFi.ResearchfiUrl>();
    }
}