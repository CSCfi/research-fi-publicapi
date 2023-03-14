using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;

namespace CSC.PublicApi.Repositories.Maps;

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        
        CreateProjection<DimOrganization, Service.Models.Organization.Organization>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.OrganizationId, opt => opt.MapFrom(src => src.OrganizationId))
            .ForMember(dst => dst.Pids, opt => opt.MapFrom(src => src.DimPids.Where(id => id.PidType == "BusinessID" || id.PidType == "PIC")))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.NameVariants, opt => opt.MapFrom(src => src.NameVariants))
            .ForMember(dst => dst.LocalOrganizationUnitId, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.LocalOrganizationUnitId) || string.Equals(src.LocalOrganizationUnitId, "-1") ? null : src.LocalOrganizationUnitId ))
            .ForMember(dst => dst.ParentId, opt => opt.MapFrom(src => src.DimOrganizationBroader == null || src.DimOrganizationBroader == -1 ? null : src.DimOrganizationBroader));
        
        CreateProjection<DimPid, Service.Models.Organization.PreferredIdentifier>()
            .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.PidContent))
            .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.PidType));
    }
}