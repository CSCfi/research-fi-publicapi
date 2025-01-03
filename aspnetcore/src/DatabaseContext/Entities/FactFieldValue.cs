using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class FactFieldValue
{
    public int DimUserProfileId { get; set; }

    public int DimFieldDisplaySettingsId { get; set; }

    public long DimNameId { get; set; }

    public int DimWebLinkId { get; set; }

    public int DimFundingDecisionId { get; set; }

    public int DimPublicationId { get; set; }

    public int DimPidId { get; set; }

    public int DimPidIdOrcidPutCode { get; set; }

    public int DimResearchActivityId { get; set; }

    public int DimEventId { get; set; }

    public int DimEducationId { get; set; }

    public int DimCompetenceId { get; set; }

    public int DimResearchCommunityId { get; set; }

    public int DimTelephoneNumberId { get; set; }

    public int DimEmailAddrressId { get; set; }

    public int DimResearcherDescriptionId { get; set; }

    public int DimIdentifierlessDataId { get; set; }

    public int DimProfileOnlyPublicationId { get; set; }

    public int DimKeywordId { get; set; }

    public int DimAffiliationId { get; set; }

    public int DimResearcherToResearchCommunityId { get; set; }

    public bool? Show { get; set; }

    public bool? PrimaryValue { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int DimResearchDatasetId { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public int DimReferencedataFieldOfScienceId { get; set; }

    public int DimProfileOnlyResearchActivityId { get; set; }

    public int DimReferencedataActorRoleId { get; set; }

    public int DimProfileOnlyDatasetId { get; set; }

    public int DimProfileOnlyFundingDecisionId { get; set; }

    public virtual DimAffiliation DimAffiliation { get; set; } = null!;

    public virtual DimCompetence DimCompetence { get; set; } = null!;

    public virtual DimEducation DimEducation { get; set; } = null!;

    public virtual DimEmailAddrress DimEmailAddrress { get; set; } = null!;

    public virtual DimEvent DimEvent { get; set; } = null!;

    public virtual DimFieldDisplaySetting DimFieldDisplaySettings { get; set; } = null!;

    public virtual DimFundingDecision DimFundingDecision { get; set; } = null!;

    public virtual DimIdentifierlessDatum DimIdentifierlessData { get; set; } = null!;

    public virtual DimKeyword DimKeyword { get; set; } = null!;

    public virtual DimName DimName { get; set; } = null!;

    public virtual DimPid DimPid { get; set; } = null!;

    public virtual DimPid DimPidIdOrcidPutCodeNavigation { get; set; } = null!;

    public virtual DimProfileOnlyDataset DimProfileOnlyDataset { get; set; } = null!;

    public virtual DimProfileOnlyFundingDecision DimProfileOnlyFundingDecision { get; set; } = null!;

    public virtual DimProfileOnlyPublication DimProfileOnlyPublication { get; set; } = null!;

    public virtual DimProfileOnlyResearchActivity DimProfileOnlyResearchActivity { get; set; } = null!;

    public virtual DimPublication DimPublication { get; set; } = null!;

    public virtual DimReferencedatum DimReferencedataActorRole { get; set; } = null!;

    public virtual DimReferencedatum DimReferencedataFieldOfScience { get; set; } = null!;

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;

    public virtual DimResearchActivity DimResearchActivity { get; set; } = null!;

    public virtual DimResearchCommunity DimResearchCommunity { get; set; } = null!;

    public virtual DimResearchDataset DimResearchDataset { get; set; } = null!;

    public virtual DimResearcherDescription DimResearcherDescription { get; set; } = null!;

    public virtual DimResearcherToResearchCommunity DimResearcherToResearchCommunity { get; set; } = null!;

    public virtual DimTelephoneNumber DimTelephoneNumber { get; set; } = null!;

    public virtual DimUserProfile DimUserProfile { get; set; } = null!;

    public virtual DimWebLink DimWebLink { get; set; } = null!;
}
