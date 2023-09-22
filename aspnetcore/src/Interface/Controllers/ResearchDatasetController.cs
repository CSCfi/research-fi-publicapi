using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using ResearchDataset = ResearchFi.ResearchDataset.ResearchDataset;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/research-datasets")]
public class ResearchDatasetController : ControllerBase
{
    private const string ApiVersion = "1.0";

    private readonly ILogger<ResearchDatasetController> _logger;
    private IResearchDatasetService _service;

    public ResearchDatasetController(
        ILogger<ResearchDatasetController> logger,
        IResearchDatasetService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// Endpoint for filtering research datasets using the specified query parameters.
    /// </summary>
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
    public async Task<IEnumerable<ResearchDataset>> Get([FromQuery] GetResearchDatasetsQueryParameters queryParameters)
    {
        var (researchDatasets, searchResult) = await _service.GetResearchDatasets(queryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return researchDatasets;
    }
}