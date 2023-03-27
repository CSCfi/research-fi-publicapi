using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.Organization;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Maps;

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateMap<GetOrganizationsQueryParameters, OrganizationSearchParameters>();
        CreateMap<Service.Models.Organization.Organization, Organization>();
    }
}