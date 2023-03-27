using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure = CSC.PublicApi.Interface.Models.Infrastructure.Infrastructure;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/infrastructures")]
public class InfrastructureController : ControllerBase
{
    private const string ApiVersion = "1.0";

    private readonly ILogger<InfrastructureController> _logger;
    private IInfrastructureService _service;

    public InfrastructureController(
        ILogger<InfrastructureController> logger,
        IInfrastructureService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// Search Infrastructures
    /// </summary>
    /// <param name="queryParameters">Query parameters for filtering the results.</param>
    /// <returns></returns>
    /*[HttpGet(Name = "GetInfrastructure")]
    [MapToApiVersion(ApiVersion)]
    [Authorize(Policy = ApiPolicies.Infrastructure.Read)]
    public async Task<IEnumerable<Infrastructure>> Get([FromQuery] GetInfrastructuresQueryParameters queryParameters)
    {
        var (infrastructures, searchResult) = await _service.GetInfrastructures(queryParameters);

        ResponseHelper.AddPaginationResponseHeaders(HttpContext, searchResult);

        return infrastructures;
    }*/
}