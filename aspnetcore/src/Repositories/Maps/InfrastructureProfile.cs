using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using Infrastructure = CSC.PublicApi.Service.Models.Infrastructure.Infrastructure;

namespace CSC.PublicApi.DataAccess.Maps;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateProjection<DimInfrastructure, Infrastructure>();
    }
}