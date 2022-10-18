using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;

namespace CSC.PublicApi.Repositories.Maps;

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateProjection<DimOrganization, Service.Models.Organization.Organization>();
    }
}