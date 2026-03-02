/*
 * API version 2.0
 */

using CSC.PublicApi.Interface.Services.V2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using ResearchDataset = ResearchFi.ResearchDataset.V2.ResearchDataset;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers.V2;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)] // At the moment, this API version is an example and should not be visible in the API documentation.
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/research-datasets")]
public class ResearchDatasetController : ControllerBase
{
    private const string ApiVersion = ApiConstants.ApiVersion2;
    private readonly ILogger<ResearchDatasetController> _logger;
    private IResearchDatasetService _service;
    private readonly IDiagnosticContext _diagnosticContext;

    public ResearchDatasetController(
        ILogger<ResearchDatasetController> logger,
        IResearchDatasetService service,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _service = service;
        _diagnosticContext = diagnosticContext;
        _diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_ResearchDataset);
    }

    /// <summary>
    /// Endpoint for filtering research datasets using the specified query parameters.
    /// </summary>
    /// <param name="researchDatasetsQueryParameters">The query parameters for filtering the results.</param>
    /// <returns>Paged search result as a collection of <see cref="ResearchDataset"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet(Name = "GetResearchDataset")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.ResearchDataset.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<ResearchDataset>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<ResearchDataset>> Get([FromQuery] GetResearchDatasetsQueryParameters researchDatasetsQueryParameters, [FromQuery] PaginationQueryParameters paginationQueryParameters)
    {
        var (researchDatasets, searchResult) = await _service.GetResearchDatasets(researchDatasetsQueryParameters, paginationQueryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return researchDatasets;
    }
}