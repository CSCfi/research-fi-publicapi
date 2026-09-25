using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CSC.PublicApi.Interface.Services;
// ProfileDataRequest/ProfileDataResponse (namespace ResearchFi.PersonPublicApi) ship in the
// CSC.ResearchFi.Mydata.PublicApiContracts NuGet package, sourced from the Mydata API repo's
// api.PublicApiContracts project. No package feed yet: to update, that repo bumps its version,
// runs `dotnet pack`, hands over the .nupkg, and we drop it into packages/ and bump the
// <PackageReference> version in Interface.csproj, then run `dotnet restore PublicApi.sln` (from
// aspnetcore/) to pick it up. Reusing the same version number instead requires
// `dotnet nuget locals global-packages --clear` first, since NuGet caches by id+version.
using ResearchFi.PersonPublicApi;
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
    /// <param name="request">Request forwarded to the person backend.</param>
    [HttpPost(Name = "PostPerson")]
    [Authorize(Policy = ApiPolicies.Person.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(ProfileDataResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> PostPerson([FromBody] ProfileDataRequest request)
    {
        var clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var result = await _personBackendClient.SearchPersonsAsync(request, clientId, HttpContext.RequestAborted);
        return Ok(result);
    }
}

