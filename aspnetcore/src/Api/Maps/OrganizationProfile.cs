using Api.Models.Entities;
using AutoMapper;

namespace Api.Maps
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateProjection<DimOrganization, Api.Models.Organization.Organization>();
        }
    }
}
