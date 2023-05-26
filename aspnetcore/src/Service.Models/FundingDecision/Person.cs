using Nest;

namespace CSC.PublicApi.Service.Models.FundingDecision;

public class Person
{
    [Keyword]
    public string? OrcId { get; set; }
    
    public string? FirstNames { get; set; }
    
    public string? LastName { get; set; }
}