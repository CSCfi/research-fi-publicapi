using Nest;

namespace CSC.PublicApi.Service.Models.Publication;

public class PreferredIdentifier
{
    [Keyword]
    public string? Content { get; set; }
    
    public string? Type { get; set; }
    
}