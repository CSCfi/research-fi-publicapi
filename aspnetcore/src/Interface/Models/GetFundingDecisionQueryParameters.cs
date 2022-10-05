using Microsoft.AspNetCore.Mvc;

namespace CSC.PublicApi.Interface.Models;

public class GetFundingDecisionQueryParameters : PaginationQueryParameters
{
    [FromQuery(Name = "name")]
    public string? Name { get; set; }

    [FromQuery(Name = "description")]
    public string? Description { get; set; }

    [FromQuery(Name = "funderName")]
    public string? FunderName { get; set; }

    [FromQuery(Name = "funderId")]
    public string? FunderId { get; set; }

    [FromQuery(Name = "funderProjectNumber")]
    public string? FunderProjectNumber { get; set; }

    [FromQuery(Name = "fundingStartYearFrom")]
    public int? FundingStartYearFrom { get; set; }

    [FromQuery(Name = "fundedOrganization")]
    public string? FundedOrganization { get; set; }

    [FromQuery(Name = "fundedOrganizationId")]
    public string? FundedOrganizationId { get; set; }

    [FromQuery(Name = "fundedPersonFirstName")]
    public string? FundedPersonFirstName { get; set; }

    [FromQuery(Name = "fundedPersonLastName")]
    public string? FundedPersonLastName { get; set; }

    [FromQuery(Name = "fundedPersonOrcid")]
    public string? FundedPersonOrcid { get; set; }

    [FromQuery(Name = "typeOfFunding")]
    public string? TypeOfFunding { get; set; }
}