using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class DimPublication
    {
        public DimPublication()
        {
            DimLocallyReportedPubInfos = new HashSet<DimLocallyReportedPubInfo>();
            DimPids = new HashSet<DimPid>();
            FactContributions = new HashSet<FactContribution>();
            FactFieldValues = new HashSet<FactFieldValue>();
            InverseDimPublicationNavigation = new HashSet<DimPublication>();
            DimFieldOfArts = new HashSet<DimFieldOfArt>();
            DimFieldOfEducations = new HashSet<DimFieldOfEducation>();
            DimFieldOfSciences = new HashSet<DimFieldOfScience>();
            DimKeywords = new HashSet<DimKeyword>();
        }

        public int Id { get; set; }
        public int? DimPublicationId { get; set; }
        public int PublicationTypeCode2 { get; set; }
        public int TargetAudienceCode { get; set; }
        public int ParentPublicationTypeCode { get; set; }
        public int ArticleTypeCode { get; set; }
        public string? AbstractFi { get; set; }
        public string? AbstractEn { get; set; }
        public string? AbstractSv { get; set; }
        public int? ReportingYear { get; set; }
        public int? PublicationYear { get; set; }
        public string PublicationId { get; set; } = null!;
        public string AuthorsText { get; set; } = null!;
        public int? NumberOfAuthors { get; set; }
        public string? PageNumberText { get; set; }
        public string? ArticleNumberText { get; set; }
        public string? IssueNumber { get; set; }
        public string? Volume { get; set; }
        public string? PublicationCountryCode { get; set; }
        public string PublicationName { get; set; } = null!;
        public string? PublicationStatusCode { get; set; }
        public string PublicationOrgId { get; set; } = null!;
        public string? ConferenceName { get; set; }
        public string? PublisherName { get; set; }
        public string? PublisherLocation { get; set; }
        public string? ParentPublicationName { get; set; }
        public string? ParentPublicationEditors { get; set; }
        public string PublicationTypeCode { get; set; } = null!;
        public bool InternationalPublication { get; set; }
        public bool InternationalCollaboration { get; set; }
        public bool BusinessCollaboration { get; set; }
        public int? LicenseCode { get; set; }
        public string? LanguageCode { get; set; }
        public string? OpenAccess { get; set; }
        public string? OpenAccessCode { get; set; }
        public string? OriginalPublicationId { get; set; }
        public string? SelfArchivedCode { get; set; }
        public decimal? ApcFeeEur { get; set; }
        public int? ApcPaymentYear { get; set; }
        public bool? PeerReviewed { get; set; }
        public bool? Report { get; set; }
        public int? ThesisTypeCode { get; set; }
        public string? Doi { get; set; }
        public string? DoiHandle { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public int DimRegisteredDataSourceId { get; set; }

        public virtual DimReferencedatum ArticleTypeCodeNavigation { get; set; } = null!;
        public virtual DimPublication? DimPublicationNavigation { get; set; }
        public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;
        public virtual DimReferencedatum ParentPublicationTypeCodeNavigation { get; set; } = null!;
        public virtual DimReferencedatum PublicationTypeCode2Navigation { get; set; } = null!;
        public virtual DimReferencedatum TargetAudienceCodeNavigation { get; set; } = null!;
        public virtual ICollection<DimLocallyReportedPubInfo> DimLocallyReportedPubInfos { get; set; }
        public virtual ICollection<DimPid> DimPids { get; set; }
        public virtual ICollection<FactContribution> FactContributions { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValues { get; set; }
        public virtual ICollection<DimPublication> InverseDimPublicationNavigation { get; set; }

        public virtual ICollection<DimFieldOfArt> DimFieldOfArts { get; set; }
        public virtual ICollection<DimFieldOfEducation> DimFieldOfEducations { get; set; }
        public virtual ICollection<DimFieldOfScience> DimFieldOfSciences { get; set; }
        public virtual ICollection<DimKeyword> DimKeywords { get; set; }
    }
}
