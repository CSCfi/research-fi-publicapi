using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.FundingCall;

namespace CSC.PublicApi.Interface.Maps;

public class FundingCallProfile : Profile
{
    public FundingCallProfile()
    {
        CreateMap<GetFundingCallQueryParameters, FundingCallSearchParameters>();
        CreateMap<Service.Models.FundingCall.FundingCall, FundingCall>();
        CreateMap<Service.Models.FundingCall.Category, Category>();
        CreateMap<Service.Models.FundingCall.Foundation, Foundation>();
    }
}