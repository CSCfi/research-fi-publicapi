/*
 * API version 1.0
 */
using CSC.PublicApi.Interface.Services.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using Infrastructure = ResearchFi.Infrastructure.Infrastructure;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/")]
public class InfrastructureController : ControllerBase
{
    private const string ApiVersion = ApiConstants.ApiVersion1;
    private readonly ILogger<InfrastructureController> _logger;
    private IInfrastructureService _infrastructureService;
    private IInfrastructureServiceService _infrastructureServiceService;
    private readonly IDiagnosticContext _diagnosticContext;

    public InfrastructureController(
        ILogger<InfrastructureController> logger,
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
    /// Endpoint for filtering infrastructures using the specified query parameters.
    /// </summary>
    /// <param name="infrastructuresQueryParameters">The query parameters for filtering the results.</param>
    /// <returns>Paged search result as a collection of <see cref="Infrastructure"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet("infrastructures", Name = "GetInfrastructures")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.Infrastructure.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Infrastructure>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<Infrastructure>> GetInfrastructures([FromQuery] GetInfrastructuresQueryParameters infrastructuresQueryParameters, [FromQuery] PaginationQueryParameters paginationQueryParameters)
    {
        var (infrastructures, searchResult) = await _infrastructureService.GetInfrastructures(infrastructuresQueryParameters, paginationQueryParameters);
        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);
        return infrastructures;
    }

/*
    /// <summary>
    /// Endpoint for getting a single infrastructure based on the URN.
    /// </summary>
    /// <param name="urn">URN of the infrastructure.</param>
    /// <returns>An instance of <see cref="Infrastructure"/>.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    /// <response code="404">Not found.</response>
    [HttpGet("infrastructures/{urn:regex(^(?!services$).+)}", Name = "GetInfrastructure")] // Exclude "services" to avoid route conflict
    [Authorize(Policy = ApiPolicies.Infrastructure.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(Infrastructure), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IResult> GetInfrastructure(string urn)
    {
        var infrastructure = await _infrastructureService.GetInfrastructure(urn);
        return infrastructure != null ? Results.Ok(infrastructure) : Results.NotFound();
    }
*/
    
    /// <summary>
    /// Endpoint for filtering infrastructure services using the specified query parameters.
    /// </summary>
    /// <param name="infrastructureServicesQueryParameters">The query parameters for filtering the results.</param>
    /// <returns>Paged search result as a collection of <see cref="ResearchFi.Infrastructure.Service"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet("infrastructures/services", Name = "GetServices")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.Infrastructure.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<ResearchFi.Infrastructure.Service>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<ResearchFi.Infrastructure.Service>> GetServices([FromQuery] GetInfrastructureServicesQueryParameters infrastructureServicesQueryParameters, [FromQuery] PaginationQueryParameters paginationQueryParameters)
    {
        var (services, searchResult) = await _infrastructureServiceService.GetInfrastructureServices(infrastructureServicesQueryParameters, paginationQueryParameters);
        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);
        return services;
    }

/*
    /// <summary>
    /// Endpoint for getting a single infrastructure service based on the URN.
    /// </summary>
    /// <param name="urn">URN of the infrastructure service.</param>
    /// <returns>An instance of <see cref="ResearchFi.Infrastructure.Service"/>.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    /// <response code="404">Not found.</response>
    [HttpGet("infrastructures/services/{urn}", Name = "GetService")]
    [Authorize(Policy = ApiPolicies.Infrastructure.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(ResearchFi.Infrastructure.Service), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IResult> GetService(string urn)
    {
        var service = await _infrastructureServiceService.GetInfrastructureService(urn);
        return service != null ? Results.Ok(service) : Results.NotFound();
    }
*/
}