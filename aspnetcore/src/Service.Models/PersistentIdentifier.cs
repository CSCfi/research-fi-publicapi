using Nest;

namespace CSC.PublicApi.Service.Models;

public class PersistentIdentifier
{
    [Keyword]
    public string? Content { get; set; }
    
    public string? Type { get; set; }
    
}