using AutoMapper;
using CSC.PublicApi.DataAccess.Entities;
using CSC.PublicApi.Service.Models.ResearchDataset;

namespace CSC.PublicApi.DataAccess.Maps;

public class ResearchDatasetProfile : Profile
{
    private const string FairDataBaseUrl = "https://etsin.fairdata.fi/dataset/";

    public ResearchDatasetProfile()
    {
        CreateProjection<DimResearchDataset, ResearchDataset>()
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.DescriptionFi, opt => opt.MapFrom(src => src.DescriptionFi))
            .ForMember(dst => dst.DescriptionSv, opt => opt.MapFrom(src => src.DescriptionSv))
            .ForMember(dst => dst.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn))
            .ForMember(dst => dst.DatasetCreated, opt => opt.MapFrom(src => src.DatasetCreated))
            // TODO: Tekijät
            // TODO: FieldOfsciences
            .ForMember(dst => dst.FieldOfSciences, opt => opt.Ignore())
            // TODO: Languages
            .ForMember(dst => dst.Languages, opt => opt.Ignore())
            .ForMember(dst => dst.AccessType, opt => opt.MapFrom(src => src.DimReferencedataAvailabilityNavigation))
            .ForMember(dst => dst.License, opt => opt.MapFrom(src => src.DimReferencedataLicenseNavigation))
            // TODO: Keywords
            .ForMember(dst => dst.Keywords, opt => opt.Ignore())
            // TODO: Liittyvät aineistot
            // TODO: Version
            .ForMember(dst => dst.VersionSet, opt => opt.Ignore())
            // TODO: Pysyvä tunniste
            .ForMember(dst => dst.PreferredIdentifiers, opt => opt.Ignore())
            .ForMember(dst => dst.LocalIdentifier, opt => opt.MapFrom(src => src.LocalIdentifier))
            .ForMember(dst => dst.FairDataUrl, opt => opt.MapFrom(src => $"{FairDataBaseUrl}{src.LocalIdentifier}"))
            ;

        // AccessType
        CreateProjection<DimReferencedatum, string>()
            .ConvertUsing(datum => datum.CodeValue);

        CreateProjection<DimReferencedatum, License>()
            .ForMember(dst => dst.Code, src => src.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.NameFi, src => src.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, src => src.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, src => src.MapFrom(src => src.NameEn));
    }
}