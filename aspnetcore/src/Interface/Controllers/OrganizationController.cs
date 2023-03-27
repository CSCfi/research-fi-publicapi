using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using Organization = ResearchFi.Organization.Organization;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/organizations")]
public class OrganizationController : ControllerBase
{
    private const string ApiVersion = "1.0";

    private readonly ILogger<OrganizationController> _logger;
    private IOrganizationService _service;

    public OrganizationController(
        ILogger<OrganizationController> logger,
        IOrganizationService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// Hae organisaatioita
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetOrganization")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.Organization.Read)]
    public async Task<IEnumerable<Organization>> Get([FromQuery] GetOrganizationsQueryParameters queryParameters)
    {
        var (organizations, searchResult) = await _service.GetOrganizations(queryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return organizations;
    }
}