namespace CSC.PublicApi.Service.Models.ResearchDataset;

public class ContributorHelper
{
    public int FactContribution_DimOrganizationId { get; set; }
    public int FactContribution_DimIdentifierlessDataId { get; set; }
    public int FactContribution_DimIdentifierlessData_DimOrganizationId { get; set; }
    public Organization? Organization_From_FactContribution_DimOrganization { get; set; }
    public Organization? Organization_From_FactContribution_DimIdentifierlessData { get; set; }
    public Organization? Organization_From_FactContribution_DimIdentifierlessData_DimOrganization { get; set; }
    public Person? Person { get; set; }
    public ReferenceData? Role { get; set; }

}