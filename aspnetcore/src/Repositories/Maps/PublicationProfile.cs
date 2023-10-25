using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Publication;
using Microsoft.Data.SqlClient;
using FactContribution = CSC.PublicApi.Service.Models.Publication.FactContribution;
using Name = CSC.PublicApi.Service.Models.Publication.Name;
using Profile = AutoMapper.Profile;
using Publication = CSC.PublicApi.Service.Models.Publication.Publication;

namespace CSC.PublicApi.Repositories.Maps;

public class PublicationProfile : Profile
{
    private const string PreprintType = "preprint";
    private const string SelfArchivedType = "self_archived";

    public PublicationProfile()
    {
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        
        CreateProjection<DimPublication, Publication>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.PublicationId))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.PublicationName))
            .ForMember(dst => dst.PublicationYear, opt => opt.MapFrom(src => (DateTime?)(src.PublicationYear.HasValue ? new DateTime(src.PublicationYear.Value,1,1,0,0,0,DateTimeKind.Utc) : null)))
            .ForMember(dst => dst.ReportingYear, opt =>opt.MapFrom(src => (DateTime?)(src.ReportingYear.HasValue ? new DateTime(src.ReportingYear.Value,1,1,0,0,0,DateTimeKind.Utc) : null)))
            .ForMember(dst => dst.ApcPaymentYear, opt =>opt.MapFrom(src => (DateTime?)(src.ApcPaymentYear.HasValue ? new DateTime(src.ApcPaymentYear.Value,1,1,0,0,0,DateTimeKind.Utc) : null)))
            .ForMember(dst => dst.AuthorsText, opt => opt.MapFrom(src => src.AuthorsText))
            .ForMember(dst => dst.DatabaseContributions, opt => opt.MapFrom(src => src.FactContributions))
            .ForMember(dst => dst.Format, opt => opt.MapFrom(src => src.PublicationTypeCode2Navigation))
            .ForMember(dst => dst.ParentPublicationType, opt => opt.MapFrom(src => src.ParentPublicationTypeCodeNavigation))
            .ForMember(dst => dst.DatabasePeerReviewed, opt => opt.MapFrom(src => src.PeerReviewed))
            .ForMember(dst => dst.TargetAudience, opt => opt.MapFrom(src => src.TargetAudienceCodeNavigation))
            .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.PublicationTypeCodeNavigation))
            .ForMember(dst => dst.ThesisType, opt => opt.MapFrom(src => src.ThesisTypeCodeNavigation))
            .ForMember(dst => dst.JournalName, opt => opt.MapFrom(src => src.JournalName)) 
            .ForMember(dst => dst.IssueNumber, opt => opt.MapFrom(src => src.IssueNumber))
            .ForMember(dst => dst.ConferenceName, opt => opt.MapFrom(src => src.ConferenceName))
            .ForMember(dst => dst.Issn1, opt => opt.MapFrom(src => src.Issn))
            .ForMember(dst => dst.Issn2, opt => opt.MapFrom(src => src.Issn2))
            .ForMember(dst => dst.Volume, opt => opt.MapFrom(src => src.Volume))
            .ForMember(dst => dst.PageNumberText, opt => opt.MapFrom(src => src.PageNumberText))
            .ForMember(dst => dst.ArticleNumberText, opt => opt.MapFrom(src => src.ArticleNumberText))
            .ForMember(dst => dst.ParentPublicationName, opt => opt.MapFrom(src => src.ParentPublicationName))
            .ForMember(dst => dst.ParentPublicationPublisher, opt => opt.MapFrom(src => src.ParentPublicationPublisher))
            .ForMember(dst => dst.OpenAccess, opt => opt.MapFrom(src => src.OpenAccess != "9" ? src.OpenAccess : null))
            .ForMember(dst => dst.PublisherOpenAccess, opt => opt.MapFrom(src => src.PublisherOpenAccessCode != "9" ? src.PublisherOpenAccessCode : null))
            .ForMember(dst => dst.Isbn1, opt => opt.MapFrom(src => src.Isbn))
            .ForMember(dst => dst.Isbn2, opt => opt.MapFrom(src => src.Isbn2))
            .ForMember(dst => dst.PublisherName, opt => opt.MapFrom(src => src.PublisherName))
            .ForMember(dst => dst.PublisherLocation, opt => opt.MapFrom(src => src.PublisherLocation))
            .ForMember(dst => dst.JufoCode, opt => opt.MapFrom(src => src.JufoCode))
            .ForMember(dst => dst.JufoClass, opt => opt.MapFrom(src => src.JufoClassCode))
            .ForMember(dst => dst.Doi, opt => opt.MapFrom(src => src.Doi))
            .ForMember(dst => dst.DoiHandle, opt => opt.MapFrom(src => src.DoiHandle))
            .ForMember(dst => dst.FieldsOfScience, opt => opt.MapFrom(src => src.FactDimReferencedataFieldOfSciences.Select(f => f.DimReferencedata)))
            .ForMember(dst => dst.FieldsOfArt, opt => opt.MapFrom(src => src.DimReferencedataNavigation))
            .ForMember(dst => dst.Keywords, opt => opt.MapFrom(src => src.DimKeywords))
            .ForMember(dst => dst.InternationalPublication, opt => opt.MapFrom(src => src.InternationalPublication != 9 ? src.InternationalPublication == 1 : (bool?)null)) // 0 = kotim. 1 ulkom. 9 = ei tietoa.
            .ForMember(dst => dst.Country, opt => opt.MapFrom(src => src.PublicationCountryCodeNavigation))
            .ForMember(dst => dst.Language, opt => opt.MapFrom(src => src.LanguageCodeNavigation))
            .ForMember(dst => dst.InternationalCollaboration, opt => opt.MapFrom(src => src.InternationalCollaboration))
            .ForMember(dst => dst.BusinessCollaboration, opt => opt.MapFrom(src => src.BusinessCollaboration))
            .ForMember(dst => dst.ApcFeeEur, opt => opt.MapFrom(src => src.ApcFeeEur))
            .ForMember(dst => dst.ArticleType, opt => opt.MapFrom(src => src.ArticleTypeCodeNavigation))
            .ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.PublicationStatusCode))
            .ForMember(dst => dst.License, opt => opt.MapFrom(src => src.LicenseCodeNavigation.Id != -1 ? src.LicenseCodeNavigation : null))
            .ForMember(dst => dst.Preprint, opt => opt.MapFrom(src => src.DimLocallyReportedPubInfos.Where(i => i.SelfArchivedType == PreprintType)))
            .ForMember(dst => dst.SelfArchived, opt => opt.MapFrom(src => src.DimLocallyReportedPubInfos.Where(i => i.SelfArchivedType == SelfArchivedType)))
            .ForMember(dst => dst.ArtPublicationTypeCategory, opt => opt.MapFrom(src => src.DimReferencedata))
            .ForMember(dst => dst.Abstract, opt => opt.MapFrom(src => src.Abstract))
            .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
            .ForMember(dst => dst.Modified, opt => opt.MapFrom(src => src.Modified))
            .ForMember(dst => dst.Issn, opt => opt.Ignore())  // Handled during in memory operations in the index repository
            .ForMember(dst => dst.Isbn, opt => opt.Ignore())  // Handled during in memory operations in the index repository
            .ForMember(dst => dst.Organizations, opt => opt.Ignore())  // Handled during in memory operations in the index repository
            .ForMember(dst => dst.Authors, opt => opt.Ignore())  // Handled during in memory operations in the index repository
            .ForMember(dst => dst.PeerReviewed, opt => opt.Ignore())  // Handled during in memory operations in the index repository
            .ForMember(dst => dst.ParentPublication, opt => opt.Ignore())  // Handled during in memory operations in the index repository
            ;
        
        CreateProjection<DimReferencedatum, ReferenceData>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.CodeValue))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));

        CreateProjection<DimLocallyReportedPubInfo, LocallyReportedPublicationInformation>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Url, opt => opt.MapFrom(src => src.SelfArchivedUrl))
            .ForMember(dst => dst.EmbargoDate, opt => opt.MapFrom(src => src.SelfArchivedEmbargoDate))
            .ForMember(dst => dst.License, opt => opt.MapFrom(src => src.SelfArchivedLicenseCodeNavigation))
            .ForMember(dst => dst.Version, opt => opt.MapFrom(src => src.SelfArchivedVersionCodeNavigation));
        
        CreateProjection<DimKeyword, Keyword>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Keyword))
            .ForMember(dst => dst.Language, opt => opt.MapFrom(src => src.Language))
            .ForMember(dst => dst.Scheme, opt => opt.MapFrom(src => src.Scheme));
        
        CreateProjection<DimFieldOfArt, ReferenceData>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.FieldId))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));
        
        CreateProjection<DimFieldOfEducation, ReferenceData>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.FieldId))
            .ForMember(dst => dst.NameFi, opt => opt.MapFrom(src => src.NameFi))
            .ForMember(dst => dst.NameSv, opt => opt.MapFrom(src => src.NameSv))
            .ForMember(dst => dst.NameEn, opt => opt.MapFrom(src => src.NameEn));
        
        CreateProjection<string, ReferenceData>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src))
            .ForMember(dst => dst.NameFi, opt => opt.Ignore())
            .ForMember(dst => dst.NameSv, opt => opt.Ignore())
            .ForMember(dst => dst.NameEn, opt => opt.Ignore());
        
        CreateProjection<int?, ReferenceData>()
            .ForMember(dst => dst.Code, opt => opt.MapFrom(src => src.ToString()))
            .ForMember(dst => dst.NameFi, opt => opt.Ignore())
            .ForMember(dst => dst.NameSv, opt => opt.Ignore())
            .ForMember(dst => dst.NameEn, opt => opt.Ignore());

        CreateProjection<DatabaseContext.Entities.FactContribution, FactContribution>()
            .ForMember(dst => dst.OrganizationId, opt => opt.MapFrom(src => src.DimOrganizationId))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.DimName))
            .ForMember(dst => dst.ArtPublicationRole, opt => opt.MapFrom(src => src.DimReferencedataActorRole))
            .ForMember(dst => dst.ContributionType, opt => opt.MapFrom(src => src.ContributionType));

        CreateProjection<DimName, Name>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.NameId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.FirstNames, opt => opt.MapFrom(src => src.FirstNames))
            .ForMember(dst => dst.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dst => dst.Orcid,
                opt => opt.MapFrom(src => 
                    src.DimKnownPersonIdConfirmedIdentityNavigation.DimPids
                        .FirstOrDefault(pid => pid.PidType == "orcid").PidContent));
        
        CreateProjection<DimPid, PersistentIdentifier>()
            .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s)
            .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.PidContent))
            .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.PidType));
    }
}
