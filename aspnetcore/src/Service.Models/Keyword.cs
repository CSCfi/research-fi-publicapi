using Nest;

namespace CSC.PublicApi.Service.Models;

public class Keyword
{
    [Keyword]
    public string? Value { get; set; }
    
    public string? Language { get; set; }

    public string? Scheme { get; set; }
}