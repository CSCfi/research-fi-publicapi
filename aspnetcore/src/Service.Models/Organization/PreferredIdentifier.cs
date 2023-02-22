using Nest;

namespace CSC.PublicApi.Service.Models.Organization;

public class PreferredIdentifier
{
    [Keyword]
    public string? Content { get; set; }
    
    public string? Type { get; set; }
    
}