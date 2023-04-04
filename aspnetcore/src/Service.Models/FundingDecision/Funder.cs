namespace CSC.PublicApi.Service.Models.FundingDecision;

public class Funder
{
    public string? NameFi { get; set; }
    
    public string? NameSv { get; set; }
    
    public string? NameEn { get; set; }
    
    public List<PersistentIdentifier>? Ids { get; set; }
}