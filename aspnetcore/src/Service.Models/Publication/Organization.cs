using Nest;

namespace CSC.PublicApi.Service.Models.Publication;

public class Organization
{
    public string? organizationId { get; set; }

    [Text(Name = "OrganizationNameFi")]
    public string? OrganizationNameFi { get; set; }

    [Text(Name = "OrganizationNameEn")]
    public string? OrganizationNameEn { get; set; }

    [Text(Name = "OrganizationNameSv")]
    public string? OrganizationNameSv { get; set; }

    public OrganizationUnit[]? organizationUnit { get; set; }
}