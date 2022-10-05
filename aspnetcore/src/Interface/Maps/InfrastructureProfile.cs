using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.Infrastructure;

namespace CSC.PublicApi.Interface.Maps;

public class InfrastructureProfileProfile : Profile
{
    public InfrastructureProfileProfile()
    {
        CreateMap<GetInfrastructuresQueryParameters, InfrastructureSearchParameters>();
        CreateMap<Service.Models.Infrastructure.Infrastructure, Infrastructure>();
    }
}