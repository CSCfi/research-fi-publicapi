using Nest;

namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class Organisation
{
    public string? organizationId { get; set; }

    [Text(Name = "OrganizationNameFi")]
    public string? NameFi { get; set; }

    [Text(Name = "OrganizationNameEn")]
    public string? NameEn { get; set; }

    [Text(Name = "OrganizationNameSv")]
    public string? NameSv { get; set; }

    [Text(Name = "OrganizationNameVariants")]
    public string? NameVariants { get; set; }

}