namespace CSC.PublicApi.Interface.Models.Publication;

public class Organization
{
    public string? Id { get; set; }

    public string? NameFi { get; set; }

    public string? NameEn { get; set; }

    public string? NameSv { get; set; }

    public OrganizationUnit[]? organizationUnit { get; set; }
}