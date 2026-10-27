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
[Route("v{version:apiVersion}/profiles")]
public class ProfileController : ControllerBase
{
    private const string ApiVersion = ApiConstants.ApiVersion1;
    private readonly ILogger<ProfileController> _logger;
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly IPersonBackendClient _personBackendClient;

    public ProfileController(ILogger<ProfileController> logger, IDiagnosticContext diagnosticContext, IPersonBackendClient personBackendClient)
    {
        _logger = logger;
        _diagnosticContext = diagnosticContext;
        _personBackendClient = personBackendClient;
        _diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_Profile);
    }

    /// <summary>
    /// Search profiles via the person backend. Response contract is not yet defined.
    /// </summary>
    /// <param name="request">Request forwarded to the person backend.</param>
    [HttpPost(Name = "PostProfile")]
    [Authorize(Policy = ApiPolicies.Profile.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(ProfileDataResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostProfile([FromBody] ProfileDataRequest request)
    {
        var clientId = HttpContext.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId")?.Value;
        var (response, errorStatusCode, errorContent, errorContentType) = await _personBackendClient.SearchPersonsAsync(request, clientId, HttpContext.RequestAborted);
        if (errorStatusCode.HasValue)
        {
            return new ContentResult
            {
                StatusCode = (int)errorStatusCode.Value,
                Content = errorContent,
                ContentType = errorContentType
            };
        }

        return Ok(response);
    }
}
