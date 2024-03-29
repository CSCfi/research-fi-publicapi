﻿using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities
{
    public partial class DimPublication
    {
        public DimPublication()
        {
            DimLocallyReportedPubInfos = new HashSet<DimLocallyReportedPubInfo>();
            DimPids = new HashSet<DimPid>();
            FactContributions = new HashSet<FactContribution>();
            FactDimReferencedataFieldOfSciences = new HashSet<FactDimReferencedataFieldOfScience>();
            FactFieldValues = new HashSet<FactFieldValue>();
            DimKeywords = new HashSet<DimKeyword>();
            DimReferencedata = new HashSet<DimReferencedatum>();
            DimReferencedataNavigation = new HashSet<DimReferencedatum>();
        }

        public int Id { get; set; }
        public int? ReportingYear { get; set; }
        public string PublicationId { get; set; } = null!;
        public int PublicationStatusCode { get; set; }
        public string PublicationOrgId { get; set; } = null!;
        public string PublicationName { get; set; } = null!;
        public string AuthorsText { get; set; } = null!;
        public int? NumberOfAuthors { get; set; }
        public string? PageNumberText { get; set; }
        public string? ArticleNumberText { get; set; }
        public string? Isbn { get; set; }
        public string? Isbn2 { get; set; }
        public int PublicationCountryCode { get; set; }
        public string? JournalName { get; set; }
        public string? Issn { get; set; }
        public string? Issn2 { get; set; }
        public string? Volume { get; set; }
        public string? IssueNumber { get; set; }
        public string? ConferenceName { get; set; }
        public string? PublisherName { get; set; }
        public string? PublisherLocation { get; set; }
        public string? ParentPublicationName { get; set; }
        public string? ParentPublicationPublisher { get; set; }
        public int PublicationTypeCode { get; set; }
        public bool? InternationalCollaboration { get; set; }
        public bool HospitalDistrictCollaboration { get; set; }
        public int InternationalPublication { get; set; }
        public bool GovermentCollaboration { get; set; }
        public bool OtherCollaboration { get; set; }
        public int LanguageCode { get; set; }
        public bool SpecialStateSubsidy { get; set; }
        public bool? BusinessCollaboration { get; set; }
        public string? DoiHandle { get; set; }
        public string? JuuliAddress { get; set; }
        public string? OriginalPublicationId { get; set; }
        public string? Doi { get; set; }
        public int? PublicationYear { get; set; }
        public int LicenseCode { get; set; }
        public decimal? ApcFeeEur { get; set; }
        public int? ApcPaymentYear { get; set; }
        public int? PublicationTypeCode2 { get; set; }
        public int? TargetAudienceCode { get; set; }
        public int? ParentPublicationTypeCode { get; set; }
        public int? ArticleTypeCode { get; set; }
        public bool? PeerReviewed { get; set; }
        public bool? Report { get; set; }
        public int ThesisTypeCode { get; set; }
        public bool? SelfArchivedCode { get; set; }
        public string SourceId { get; set; } = null!;
        public string? SourceDescription { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public int DimRegisteredDataSourceId { get; set; }
        public string? OpenAccess { get; set; }
        public int PublisherOpenAccessCode { get; set; }
        public string? Abstract { get; set; }
        public int DimPublicationChannelId { get; set; }
        public int JufoClass { get; set; }

        public virtual DimReferencedatum? ArticleTypeCodeNavigation { get; set; }
        public virtual DimPublicationChannel DimPublicationChannel { get; set; } = null!;
        public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;
        public virtual DimReferencedatum JufoClassNavigation { get; set; } = null!;
        public virtual DimReferencedatum LanguageCodeNavigation { get; set; } = null!;
        public virtual DimReferencedatum LicenseCodeNavigation { get; set; } = null!;
        public virtual DimReferencedatum? ParentPublicationTypeCodeNavigation { get; set; }
        public virtual DimReferencedatum PublicationCountryCodeNavigation { get; set; } = null!;
        public virtual DimReferencedatum PublicationStatusCodeNavigation { get; set; } = null!;
        public virtual DimReferencedatum? PublicationTypeCode2Navigation { get; set; }
        public virtual DimReferencedatum PublicationTypeCodeNavigation { get; set; } = null!;
        public virtual DimReferencedatum PublisherOpenAccessCodeNavigation { get; set; } = null!;
        public virtual DimReferencedatum? TargetAudienceCodeNavigation { get; set; }
        public virtual DimReferencedatum ThesisTypeCodeNavigation { get; set; } = null!;
        public virtual ICollection<DimLocallyReportedPubInfo> DimLocallyReportedPubInfos { get; set; }
        public virtual ICollection<DimPid> DimPids { get; set; }
        public virtual ICollection<FactContribution> FactContributions { get; set; }
        public virtual ICollection<FactDimReferencedataFieldOfScience> FactDimReferencedataFieldOfSciences { get; set; }
        public virtual ICollection<FactFieldValue> FactFieldValues { get; set; }

        public virtual ICollection<DimKeyword> DimKeywords { get; set; }
        public virtual ICollection<DimReferencedatum> DimReferencedata { get; set; }
        public virtual ICollection<DimReferencedatum> DimReferencedataNavigation { get; set; }
    }
}
