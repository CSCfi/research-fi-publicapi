/*
 * API version 1.0
 */
using CSC.PublicApi.Interface.Services.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using Infrastructure =ResearchFi.Infrastructure.Infrastructure;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/infrastructures-export")]
public class InfrastructureExportController : ControllerBase
{
    private const string ApiVersion = ApiConstants.ApiVersion1;
    private readonly ILogger<InfrastructureExportController> _logger;
    private IInfrastructureService _infrastructureService;
    private IInfrastructureServiceService _infrastructureServiceService;
    private readonly IDiagnosticContext _diagnosticContext;

    public InfrastructureExportController(
        ILogger<InfrastructureExportController> logger,
        IInfrastructureService infrastructureService,
        IInfrastructureServiceService infrastructureServiceService,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _infrastructureService = infrastructureService;
        _infrastructureServiceService = infrastructureServiceService;
        _diagnosticContext = diagnosticContext;
        _diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_Infrastructure);
    }

    /// <summary>
    /// Endpoint for bypassing the limit of 10000 records for infrastructures.
    /// </summary>
    /// <param name="infrastructuresQueryParameters">The query parameters for filtering the results.</param>
    /// <returns>Paged search result as a collection of <see cref="Infrastructure"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet(Name = "GetInfrastructuresExport")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.Infrastructure.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Infrastructure>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    // [ApiExplorerSettings(IgnoreApi = true)] // Hidden
    public async Task<IEnumerable<Infrastructure>> Get([FromQuery] GetInfrastructuresQueryParameters infrastructuresQueryParameters, [FromQuery] SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (infrastructures, searchAfterResult) = await _infrastructureService.GetInfrastructuresSearchAfter(infrastructuresQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfterResult);

        return infrastructures;
    }

    /// <summary>
    /// Endpoint for bypassing the limit of 10000 records for infrastructure services.
    /// </summary>
    /// <param name="infrastructureServicesQueryParameters">The query parameters for filtering the results.</param>
    /// <returns>Paged search result as a collection of <see cref="ResearchFi.Infrastructure.Service"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet("services", Name = "GetInfrastructuresServicesExport")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.Infrastructure.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Infrastructure>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    // [ApiExplorerSettings(IgnoreApi = true)] // Hidden
    public async Task<IEnumerable<ResearchFi.Infrastructure.Service>> GetServices([FromQuery] GetInfrastructureServicesQueryParameters infrastructureServicesQueryParameters, [FromQuery] SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (services, searchAfterResult) = await _infrastructureServiceService.GetInfrastructureServicesSearchAfter(infrastructureServicesQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfterResult);

        return services;
    }
}