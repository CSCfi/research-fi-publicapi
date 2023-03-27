using Nest;

namespace CSC.PublicApi.Service.Models.Publication;

public class Organization
{
    [Keyword]
    public string? Id { get; set; }

    public string? NameFi { get; set; }

    public string? NameEn { get; set; }

    public string? NameSv { get; set; }

    public List<OrganizationUnit>? Units { get; set; }
}