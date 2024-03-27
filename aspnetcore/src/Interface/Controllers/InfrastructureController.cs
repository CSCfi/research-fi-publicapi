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