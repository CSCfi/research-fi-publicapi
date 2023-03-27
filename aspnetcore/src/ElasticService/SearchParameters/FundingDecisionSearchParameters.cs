namespace CSC.PublicApi.ElasticService.SearchParameters;

public class FundingDecisionSearchParameters
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? FunderName { get; set; }

    public string? FunderId { get; set; }

    public string? FunderProjectNumber { get; set; }

    public int? FundingStartYearFrom { get; set; }

    public string? FundedOrganization { get; set; }

    public string? FundedOrganizationId { get; set; }

    public string? FundedPersonFirstName { get; set; }

    public string? FundedPersonLastName { get; set; }

    public string? FundedPersonOrcid { get; set; }

    public string? TypeOfFunding { get; set; }
}