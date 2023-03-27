using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchDataset = CSC.PublicApi.Interface.Models.ResearchDataset.ResearchDataset;

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
    /// Hae aineistoja
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetResearchDataset")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.ResearchDataset.Read)]
    public async Task<IEnumerable<ResearchDataset>> Get([FromQuery] GetResearchDatasetsQueryParameters queryParameters)
    {
        var (researchDatasets, searchResult) = await _service.GetResearchDatasets(queryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return researchDatasets;
    }
}