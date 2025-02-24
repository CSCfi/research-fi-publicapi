using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.Infrastructure;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Maps;

public class InfrastructureProfileProfile : Profile
{
    public InfrastructureProfileProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        
        CreateMap<GetInfrastructuresQueryParameters, InfrastructureSearchParameters>();
        CreateMap<Service.Models.Infrastructure.Infrastructure, Infrastructure>();
        CreateMap<Service.Models.ResearchfiUrl, ResearchFi.ResearchfiUrl>();
    }
}