using Nest;

namespace CSC.PublicApi.Interface.Models.Publication;

public class OrganizationUnit
{
    [Text(Name = "OrgUnitId")]
    public string? OrgUnitId { get; set; }
    public string? organizationUnitNameFi { get; set; }
    public string? organizationUnitNameEn { get; set; }
    public string? organizationUnitNameSv { get; set; }
    public Person[]? person { get; set; }
}