using Nest;

namespace CSC.PublicApi.Service.Models.Publication;

public class OrganizationUnit
{
    [Ignore]
    public int? DatabaseId { get; set; }
    
    [Keyword]
    public string? Id { get; set; }
    
    public string? NameFi { get; set; }
    
    public string? NameEn { get; set; }
    
    public string? NameSv { get; set; }
    
}