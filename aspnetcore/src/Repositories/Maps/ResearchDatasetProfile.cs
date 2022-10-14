using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models.ResearchDataset;
using Language = CSC.PublicApi.Service.Models.ResearchDataset.Language;

namespace CSC.PublicApi.Repositories.Maps;

public class ResearchDatasetProfile : Profile
{
    private const string FairDataBaseUrl = "https://etsin.fairdata.fi/dataset/";

    public ResearchDatasetProfile()
    {
        CreateProjection<DimResearchDataset, ResearchDataset>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.DescriptionFi, opt => opt.MapFrom(src => src.DescriptionFi))
            .ForMember(dst => dst.DescriptionSv, opt => opt.MapFrom(src => src.DescriptionSv))
            .ForMember(dst => dst.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn))
            .ForMember(dst => dst.DatasetCreated, opt => opt.MapFrom(src => src.DatasetCreated))
            .ForMember(dst => dst.Contributors, opt => opt.MapFrom(src => src.FactContributions))
            .ForMember(dst => dst.FieldOfSciences, opt => opt.MapFrom(src => src.DimFieldOfSciences))
            .ForMember(dst => dst.Languages, opt => opt.MapFrom(src => src.DimReferencedata))
            .ForMember(dst => dst.AccessType, opt => opt.MapFrom(src => src.DimReferencedataAvailabilityNavigation))
            .ForMember(dst => dst.License, opt => opt.MapFrom(src => src.DimReferencedataLicenseNavigation))
            .ForMember(dst => dst.Keywords, opt => opt.MapFrom(src => src.DimKeywords))
            .ForMember(dst => dst.DatasetRelations, opt => opt.MapFrom(src => src.BrDatasetDatasetRelationshipDimResearchDatasets))
            .ForMember(dst => dst.PreferredIdentifiers, opt => opt.MapFrom(src => src.DimPids))
            .ForMember(dst => dst.LocalIdentifier, opt => opt.MapFrom(src => src.LocalIdentifier))
            .ForMember(dst => dst.FairDataUrl, opt => opt.MapFrom(src => $"{FairDataBaseUrl}{src.LocalIdentifier}"))
            .ForMember(dst => dst.ResearchDataCatalog, opt => opt.MapFrom(src => src.DimResearchDataCatalog))
            .ForMember(dst => dst.OutgoingVersions, opt => opt.MapFrom(src => src.BrDatasetDatasetRelationshipDimResearchDatasets))
            .ForMember(dst => dst.IncomingVersions, opt => opt.MapFrom(src => src.BrDatasetDatasetRelationshipDimResearchDatasetId2Navigations))
            .ForMember(dst => dst.VersionSet, opt => opt.Ignore())
            .ForMember(dst => dst.IsLatestVersion, opt => opt.Ignore())
            ;

        // AccessType
        CreateProjection<DimReferencedatum, Availability>()
            .ForMember(dst => dst.Code, src => src.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.NameFi, src => src.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, src => src.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, src => src.MapFrom(src => src.NameEn));

        CreateProjection<DimReferencedatum, License>()
            .ForMember(dst => dst.Code, src => src.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.NameFi, src => src.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, src => src.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, src => src.MapFrom(src => src.NameEn));

        CreateProjection<DimReferencedatum, Language>()
            .ForMember(dst => dst.Code, src => src.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.NameFi, src => src.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, src => src.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, src => src.MapFrom(src => src.NameEn));

        CreateProjection<DimFieldOfScience, FieldOfScience>()
            .ForMember(dst => dst.FieldId, src => src.MapFrom(src => src.FieldId))
            .ForMember(dst => dst.NameFi, src => src.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, src => src.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, src => src.MapFrom(src => src.NameEn));

        CreateProjection<DimKeyword, Keyword>()
            .ForMember(dst => dst.Value, src => src.MapFrom(src => src.Keyword))
            .ForMember(dst => dst.Language, src => src.MapFrom(src => src.Language))
            .ForMember(dst => dst.Scheme, src => src.MapFrom(src => src.Scheme));

        CreateProjection<FactContribution, Contributor>()
            .ForMember(dst => dst.Organisation, src => src.MapFrom(src => src.DimOrganization))
            .ForMember(dst => dst.Person, src => src.MapFrom(src => src.DimName))
            .ForMember(dst => dst.Role, src => src.MapFrom(src => src.DimReferencedataActorRole));

        CreateProjection<DimReferencedatum, Role>()
            .ForMember(dst => dst.NameFi, src => src.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, src => src.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, src => src.MapFrom(src => src.NameEn));

        CreateProjection<DimOrganization, Organisation>()
            .ForMember(dst => dst.organizationId, src => src.MapFrom(src => src.OrganizationId))
            .ForMember(dst => dst.NameFi, src => src.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, src => src.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, src => src.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.NameVariants, src => src.MapFrom(src => src.NameVariants));

        CreateProjection<DimName, Person>()
            .ForMember(dst => dst.FirstNames, src => src.MapFrom(src => src.FirstNames))
            .ForMember(dst => dst.LastName, src => src.MapFrom(src => src.LastName))
            .ForMember(dst => dst.FullName, src => src.MapFrom(src => src.FullName));

        CreateProjection<BrDatasetDatasetRelationship, DatasetRelation>()
            .ForMember(dst => dst.Id, src => src.MapFrom(src => src.DimResearchDatasetId2))
            .ForMember(dst => dst.Type, src => src.MapFrom(src => src.Type));

        CreateProjection<BrDatasetDatasetRelationship, VersionBridge>()
            .ForMember(dst => dst.Identifier, src => src.MapFrom(src => src.DimResearchDatasetId))
            .ForMember(dst => dst.Identifier2, src => src.MapFrom(src => src.DimResearchDatasetId2))
            .ForMember(dst => dst.VersionNumber, src => src.MapFrom(src => src.DimResearchDataset.VersionInfo))
            .ForMember(dst => dst.VersionNumber2, src => src.MapFrom(src => src.DimResearchDatasetId2Navigation.VersionInfo));

        CreateProjection<DimResearchDataCatalog, ResearchDataCatalog>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.SourceId, opt => opt.MapFrom(src => src.SourceId))
            .ForMember(dst => dst.SourceDescription, opt => opt.MapFrom(src => src.SourceDescription));

        CreateProjection<DimPid, PreferredIdentifier>()
            .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.PidContent))
            .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.PidType));
    }
}
