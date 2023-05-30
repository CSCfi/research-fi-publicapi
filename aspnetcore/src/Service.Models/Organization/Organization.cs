using CSC.PublicApi.Service.Models.Publication;

namespace CSC.PublicApi.Service.Models.Organization;

public class Organization
{
    public int Id { get; set; }
    
    public int? ParentId { get; set; }

    public string? OrganizationId { get; set; }
    
    public string? LocalOrganizationUnitId { get; set; }
    
    public List<PersistentIdentifier>? Pids { get; set; }

    public string? NameFi { get; set; }

    public string? NameEn { get; set; }

    public string? NameSv { get; set; }

    public string? NameVariants { get; set; }
    
    public string? CountryCode { get; set; }

    public Publication.Organization ToMainOrganization()
    {
        return new Publication.Organization
        {
            Id = OrganizationId,
            NameFi = NameFi,
            NameSv = NameSv,
            NameEn = NameEn
        };
    }
    
    public OrganizationUnit ToOrganizationUnit()
    {
        return new OrganizationUnit
        {
            DatabaseId = Id,
            Id = LocalOrganizationUnitId,
            NameFi = NameFi,
            NameSv = NameSv,
            NameEn = NameEn
        };
    }

    public FundingDecision.Organization ToFundingDecisionOrganization()
    {
        return new FundingDecision.Organization
        {
            NameFi = NameFi,
            NameSv = NameSv,
            NameEn = NameEn,
            CountryCode = CountryCode,
            Pids = Pids
        };
    }
}