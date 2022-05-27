using Api.Models.Entities;
using AutoMapper;

namespace Api.Maps
{
    public class InfrastructureProfile : Profile
    {
        public InfrastructureProfile()
        {
            CreateProjection<DimInfrastructure, Api.Models.Infrastructure.Infrastructure>();
        }
    }
}
