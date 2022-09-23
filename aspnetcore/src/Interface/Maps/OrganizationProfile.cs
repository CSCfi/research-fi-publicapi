using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.Organization;

namespace CSC.PublicApi.Interface.Maps;

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateMap<GetOrganizationsQueryParameters, OrganizationSearchParameters>();
        CreateMap<Service.Models.Organization.Organization, Organization>();
    }
}