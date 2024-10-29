using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using ResearchDataset = ResearchFi.ResearchDataset.ResearchDataset;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/research-datasets-export")]
public class ResearchDatasetExportController : ControllerBase
{
    private const string ApiVersion = "1.0";

    private readonly ILogger<ResearchDatasetExportController> _logger;
    private IResearchDatasetService _service;
    private readonly IDiagnosticContext _diagnosticContext;

    public ResearchDatasetExportController(
        ILogger<ResearchDatasetExportController> logger,
        IResearchDatasetService service,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _service = service;
        _diagnosticContext = diagnosticContext;
        _diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_ResearchDataset);
    }

    /// <summary>
    /// Endpoint for bypassing the limit of 10000 records for research datasets.
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="ResearchDataset"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet(Name = "GetResearchDatasetExport")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.ResearchDataset.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<ResearchDataset>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<ResearchDataset>> Get([FromQuery] GetResearchDatasetsQueryParameters researchDatasetsQueryParameters, [FromQuery] SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (researchDatasets, searchAfter) = await _service.GetResearchDatasetsSearchAfter(researchDatasetsQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfter);

        return researchDatasets;
    }
}