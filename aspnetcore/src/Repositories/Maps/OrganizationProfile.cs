using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;

namespace CSC.PublicApi.DataAccess.Maps;

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateProjection<DimOrganization, Service.Models.Organization.Organization>();
    }
}