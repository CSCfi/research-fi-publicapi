using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using Infrastructure =ResearchFi.Infrastructure.Infrastructure;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/infrastructures")]
public class InfrastructureController : ControllerBase
{
    private const string ApiVersion = "1.0";

    private readonly ILogger<InfrastructureController> _logger;
    private IInfrastructureService _service;
    private readonly IDiagnosticContext _diagnosticContext;

    public InfrastructureController(
        ILogger<InfrastructureController> logger,
        IInfrastructureService service,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _service = service;
        _diagnosticContext = diagnosticContext;
        _diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_Infrastructure);
    }

    /// <summary>
    /// Search Infrastructures
    /// </summary>
    /// <param name="infrastructuresQueryParameters">The query parameters for filtering the results.</param>
    /// <returns>Paged search result as a collection of <see cref="Infrastructure"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet(Name = "GetInfrastructure")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.Infrastructure.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Infrastructure>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    [ApiExplorerSettings(IgnoreApi = true)] // Hidden
    public async Task<IEnumerable<Infrastructure>> Get([FromQuery] GetInfrastructuresQueryParameters infrastructuresQueryParameters, [FromQuery] PaginationQueryParameters paginationQueryParameters)
    {
        var (infrastructures, searchResult) = await _service.GetInfrastructures(infrastructuresQueryParameters, paginationQueryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return infrastructures;
    }
}