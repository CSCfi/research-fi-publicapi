using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.CodeList;
using ResearchFi.FundingCall;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Maps;

public class FundingCallProfile : Profile
{
    public FundingCallProfile()
    {
        CreateMap<GetFundingCallQueryParameters, FundingCallSearchParameters>();
        CreateMap<Service.Models.FundingCall.FundingCall, FundingCall>();
        CreateMap<Service.Models.FundingCall.Foundation, Foundation>();
        CreateMap<Service.Models.ReferenceData, Category>();
    }
}