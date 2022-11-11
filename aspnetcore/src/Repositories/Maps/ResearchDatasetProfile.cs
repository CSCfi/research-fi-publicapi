using AutoMapper;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models.ResearchDataset;
using Language = CSC.PublicApi.Service.Models.ResearchDataset.Language;

namespace CSC.PublicApi.Repositories.Maps;

public class ResearchDatasetProfile : Profile
{
    private const string DataSetTypeVersion = "version";

    public ResearchDatasetProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        
        CreateProjection<DimResearchDataset, ResearchDataset>()
            .ForMember(dst => dst.DatabaseId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.DescriptionFi, opt => opt.MapFrom(src => src.DescriptionFi))
            .ForMember(dst => dst.DescriptionSv, opt => opt.MapFrom(src => src.DescriptionSv))
            .ForMember(dst => dst.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn))
            .ForMember(dst => dst.DatasetCreated, opt => opt.MapFrom(src => src.DatasetCreated))
            .ForMember(dst => dst.Contributors, opt => opt.MapFrom(src => src.FactContributions.Where(fc => fc.DimOrganizationId != -1 || fc.DimNameId != -1)))
            .ForMember(dst => dst.FieldOfSciences, opt => opt.MapFrom(src => src.DimFieldOfSciences))
            .ForMember(dst => dst.Languages, opt => opt.MapFrom(src => src.DimReferencedata))
            .ForMember(dst => dst.AccessType, opt => opt.MapFrom(src => src.DimReferencedataAvailabilityNavigation))
            .ForMember(dst => dst.License, opt => opt.MapFrom(src => src.DimReferencedataLicenseNavigation))
            .ForMember(dst => dst.Keywords, opt => opt.MapFrom(src => src.DimKeywords))
            .ForMember(dst => dst.PreferredIdentifiers, opt => opt.MapFrom(src => src.DimPids))
            .ForMember(dst => dst.Identifier, opt => opt.MapFrom(src => src.LocalIdentifier))
            .ForMember(dst => dst.ResearchDataCatalog, opt => opt.MapFrom(src => src.DimResearchDataCatalog))
            .ForMember(dst => dst.OutgoingDatasetRelations, opt => opt.MapFrom(src => src.BrDatasetDatasetRelationshipDimResearchDatasets))
            .ForMember(dst => dst.IncomingDatasetVersionRelations, opt => opt.MapFrom(src => src.BrDatasetDatasetRelationshipDimResearchDatasetId2Navigations.Where(s => s.Type == DataSetTypeVersion)))
            .ForMember(dst => dst.DatasetRelations, opt => opt.Ignore())
            .ForMember(dst => dst.VersionSet, opt => opt.Ignore())
            .ForMember(dst => dst.IsLatestVersion, opt => opt.Ignore())
            ;

        // AccessType
        CreateProjection<DimReferencedatum, Availability>()
            .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));

        CreateProjection<DimReferencedatum, License>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));

        CreateProjection<DimReferencedatum, Language>()
            .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));

        CreateProjection<DimFieldOfScience, FieldOfScience>()
            .ForMember(dst => dst.FieldId, opt => opt.MapFrom(src => src.FieldId))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));

        CreateProjection<DimKeyword, Keyword>()
            .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Keyword))
            .ForMember(dst => dst.Language, opt => opt.MapFrom(src => src.Language));

        CreateProjection<FactContribution, Contributor>()
            .ForMember(dst => dst.Organisation, opt => opt.MapFrom(src => src.DimOrganization))
            .ForMember(dst => dst.Person, opt => opt.MapFrom(src => src.DimName))
            .ForMember(dst => dst.Role, opt => opt.MapFrom(src => src.DimReferencedataActorRole));

        CreateProjection<DimReferencedatum, Role>()
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));

        CreateProjection<DimOrganization, Organisation>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.Pids, opt => opt.MapFrom(src => src.DimPids.Where(id => id.PidType == "BusinessID" || id.PidType == "PIC")))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dst => dst.NameVariants, opt => opt.MapFrom(src => src.NameVariants));

        CreateProjection<DimName, Person>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.FullName));

        CreateProjection<BrDatasetDatasetRelationship, DatasetRelationBridge>()
            .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dst => dst.DatabaseId, opt => opt.MapFrom(src => src.DimResearchDatasetId))
            .ForMember(dst => dst.DatabaseId2, opt => opt.MapFrom(src => src.DimResearchDatasetId2))
            .ForMember(dst => dst.VersionNumber, opt => opt.MapFrom(src => src.DimResearchDataset.VersionInfo))
            .ForMember(dst => dst.VersionNumber2, opt => opt.MapFrom(src => src.DimResearchDatasetId2Navigation.VersionInfo))
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.DimResearchDataset.LocalIdentifier))
            .ForMember(dst => dst.Id2, opt => opt.MapFrom(src => src.DimResearchDatasetId2Navigation.LocalIdentifier));

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
