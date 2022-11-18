using Nest;

namespace CSC.PublicApi.Service.Models.Publication;

public class OrganizationUnit
{
    public string? Id { get; set; }
    public string? NameFi { get; set; }
    public string? NameEn { get; set; }
    public string? NameSv { get; set; }
    public Person[]? Person { get; set; }
}