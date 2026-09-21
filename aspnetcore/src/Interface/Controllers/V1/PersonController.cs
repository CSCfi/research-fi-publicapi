using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CSC.PublicApi.Interface.Services;
using ResearchFi.Query;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiVersion)]
[Route("v{version:apiVersion}/persons")]
public class PersonController : ControllerBase
{
    private const string ApiVersion = ApiConstants.ApiVersion1;
    private readonly ILogger<PersonController> _logger;
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly IPersonBackendClient _personBackendClient;

    public PersonController(ILogger<PersonController> logger, IDiagnosticContext diagnosticContext, IPersonBackendClient personBackendClient)
    {
        _logger = logger;
        _diagnosticContext = diagnosticContext;
        _personBackendClient = personBackendClient;
        _diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_Person);
    }

    /// <summary>
    /// Search persons via the person backend. Response contract is not yet defined.
    /// </summary>
    /// <param name="getPersonsQueryParameters">Search parameters provided in the request body.</param>
    [HttpPost(Name = "PostPerson")]
    [Authorize(Policy = ApiPolicies.Person.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PostPerson([FromBody] GetPersonsQueryParameters getPersonsQueryParameters)
    {
        var result = await _personBackendClient.SearchPersonsAsync(getPersonsQueryParameters, HttpContext.RequestAborted);
        return Ok(result);
    }
}

