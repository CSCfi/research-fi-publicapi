﻿using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.Organization;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Maps;

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        
        CreateMap<GetOrganizationsQueryParameters, OrganizationSearchParameters>();
        CreateMap<Service.Models.Organization.Organization, Organization>();
        CreateMap<Service.Models.ResearchfiUrl, ResearchFi.ResearchfiUrl>();
    }
}