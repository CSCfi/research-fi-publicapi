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
    /// <param name="queryParameters"></param>
    /// <returns></returns>
    /*[HttpGet(Name = "GetOrganization")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.Organization.Read)]
    public async Task<IEnumerable<Organization>> Get([FromQuery] GetOrganizationsQueryParameters queryParameters)
    {
        var (organizations, searchResult) = await _service.GetOrganizations(queryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return organizations;
    }*/
}