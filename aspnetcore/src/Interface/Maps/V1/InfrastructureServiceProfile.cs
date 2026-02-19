/*
 * API version 1.0
 */
using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Maps.V1;

public class InfrastructureServiceProfile : Profile
{
    public InfrastructureServiceProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;

        CreateMap<GetInfrastructureServicesQueryParameters, InfrastructureServiceSearchParameters>();
        CreateMap<Service.Models.Infrastructure.Service, ResearchFi.Infrastructure.Service>();
        CreateMap<Service.Models.Infrastructure.ServiceInfrastructure, ResearchFi.Infrastructure.ServiceInfrastructure>();
    }
}