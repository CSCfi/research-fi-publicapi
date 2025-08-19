using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using Infrastructure =ResearchFi.Infrastructure.Infrastructure;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiConstants.ApiVersion1)]
[Route("v{version:apiVersion}/infrastructures-export")]
public class InfrastructureExportController : ControllerBase
{
    private readonly ILogger<InfrastructureExportController> _logger;
    private IInfrastructureService _service;
    private readonly IDiagnosticContext _diagnosticContext;

    public InfrastructureExportController(
        ILogger<InfrastructureExportController> logger,
        IInfrastructureService service,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _service = service;
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
    [Authorize(Policy = ApiPolicies.Infrastructure.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Infrastructure>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    // [ApiExplorerSettings(IgnoreApi = true)] // Hidden
    public async Task<IEnumerable<Infrastructure>> Get([FromQuery] GetInfrastructuresQueryParameters infrastructuresQueryParameters, [FromQuery] SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (infrastructures, searchAfterResult) = await _service.GetInfrastructuresSearchAfter(infrastructuresQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfterResult);

        return infrastructures;
    }
}