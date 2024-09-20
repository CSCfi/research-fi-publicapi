using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using Organization = ResearchFi.Organization.Organization;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/organizations")]
public class OrganizationController : ControllerBase
{
    private const string ApiVersion = "1.0";

    private readonly ILogger<OrganizationController> _logger;
    private IOrganizationService _service;
    private readonly IDiagnosticContext _diagnosticContext;

    public OrganizationController(
        ILogger<OrganizationController> logger,
        IOrganizationService service,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _service = service;
        _diagnosticContext = diagnosticContext;
        _diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_Organization);
    }

    /// <summary>
    /// Hae organisaatioita
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="Organization"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet(Name = "GetOrganization")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.Organization.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Organization>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<Organization>> Get([FromQuery] GetOrganizationsQueryParameters organizationsQueryParameters, [FromQuery] PaginationQueryParameters paginationQueryParameters)
    {
        var (organizations, searchResult) = await _service.GetOrganizations(organizationsQueryParameters, paginationQueryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return organizations;
    }
}