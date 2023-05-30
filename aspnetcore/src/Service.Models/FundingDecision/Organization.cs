namespace CSC.PublicApi.Service.Models.FundingDecision;

public class Organization
{
    public List<PersistentIdentifier>? Pids { get; set; }

    public string? NameFi { get; set; }

    public string? NameEn { get; set; }

    public string? NameSv { get; set; }

    public string? CountryCode { get; set; }

    public bool IsFinnishOrganization => Pids != null && Pids.Any(p => p.Type == "BusinessID");
}