using Nest;

namespace CSC.PublicApi.Service.Models.Publication;

public class Organization
{
    public string? Id { get; set; }

    public string? NameFi { get; set; }

    public string? NameEn { get; set; }

    public string? NameSv { get; set; }

    public OrganizationUnit[]? Unit { get; set; }
}