using ResearchFi.Publication;
using ResearchFi.FundingCall;
using ResearchFi.FundingDecision;
using ResearchFi.ResearchDataset;
using ResearchFi.Infrastructure;
using Organization = ResearchFi.Organization.Organization;
using CSC.PublicApi.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchFi.Query;
using Serilog;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[ApiVersion(ApiConstants.ApiVersion1)]
[Route("v{version:apiVersion}/export")]

public class ExportController : ControllerBase
{
    private readonly ILogger<PublicationController> _logger;
    private readonly IPublicationService _publicationService;
    private readonly IFundingDecisionService _fundingDecisionService;
    private readonly IFundingCallService _fundingCallService;
    private readonly IResearchDatasetService _researchDatasetService;
    private readonly IOrganizationService _organizationService;
    private readonly IInfrastructureService _infrastructureService;
    private readonly IDiagnosticContext _diagnosticContext;

    public ExportController(
        ILogger<PublicationController> logger,
        IPublicationService publicationService,
        IFundingDecisionService fundingDecisionService,
        IFundingCallService fundingCallService,
        IResearchDatasetService researchDatasetService,
        IOrganizationService organizationService,
        IInfrastructureService infrastructureService,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _publicationService = publicationService;
        _fundingDecisionService = fundingDecisionService;
        _fundingCallService = fundingCallService;
        _researchDatasetService = researchDatasetService;
        _organizationService = organizationService;
        _infrastructureService = infrastructureService;
        _diagnosticContext = diagnosticContext;
        //_diagnosticContext.Set(ApiConstants.LogResourceType_PropertyName, ApiConstants.LogResourceType_Publication);
    }

    /// <summary>
    /// Experimental endpoint for getting more than 10000 publications.
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="Publication"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet("publications")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Publication>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<Publication>> Get([FromQuery] GetPublicationsQueryParameters publicationsQueryParameters, [FromQuery] SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (publications, searchAfter) = await _publicationService.GetPublicationsSearchAfter(publicationsQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfter);

        return publications;
    }


    /// <summary>
    /// Experimental endpoint for getting more than 10000 funding decisions.
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="FundingDecision"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet("funding-decisions")]
    [Authorize(Policy = ApiPolicies.FundingDecision.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<FundingDecision>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<FundingDecision>> Get([FromQuery] GetFundingDecisionQueryParameters fundingDecisionQueryParameters, [FromQuery] SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (fundingDecisions, searchAfter) = await _fundingDecisionService.GetFundingDecisionsSearchAfter(fundingDecisionQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfter);

        return fundingDecisions;
    }


    /// <summary>
    /// Experimental endpoint for getting more than 10000 funding calls.
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="FundingCall"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet("funding-calls")]
    [Authorize(Policy = ApiPolicies.FundingCall.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<FundingCall>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<FundingCall>> Get([FromQuery] GetFundingCallQueryParameters fundingCallQueryParameters, [FromQuery] SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (fundingCalls, searchAfter) = await _fundingCallService.GetFundingCallsSearchAfter(fundingCallQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfter);

        return fundingCalls;
    }


    /// <summary>
    /// Experimental endpoint for getting more than 10000 research datasets.
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="ResearchDataset"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet("research-datasets")]
    [Authorize(Policy = ApiPolicies.ResearchDataset.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<ResearchDataset>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<ResearchDataset>> Get([FromQuery] GetResearchDatasetsQueryParameters researchDatasetsQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (researchDatasets, searchAfter) = await _researchDatasetService.GetResearchDatasetsSearchAfter(researchDatasetsQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfter);

        return researchDatasets;
    }


    /// <summary>
    /// Experimental endpoint for getting more than 10000 infrastructures.
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="Infrastructure"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet("infrastructures")]
    [Authorize(Policy = ApiPolicies.Infrastructure.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Infrastructure>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<Infrastructure>> Get([FromQuery] GetInfrastructuresQueryParameters infrastructuresQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (infrastructures, searchAfter) = await _infrastructureService.GetInfrastructuresSearchAfter(infrastructuresQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfter);

        return infrastructures;
    }


    /// <summary>
    /// Experimental endpoint for getting more than 10000 organizations.
    /// </summary>
    /// <returns>Paged search result as a collection of <see cref="Organization"/> objects.</returns>
    /// <response code="200">Ok.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden.</response>
    [HttpGet("organizations")]
    [Authorize(Policy = ApiPolicies.Organization.Read)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<Organization>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void),StatusCodes.Status403Forbidden)]
    public async Task<IEnumerable<Organization>> Get([FromQuery] GetOrganizationsQueryParameters organizationsQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var (organizations, searchAfter) = await _organizationService.GetOrganizationsSearchAfter(organizationsQueryParameters, searchAfterQueryParameters);

        ResponseHelper.AddPaginationResponseHeadersSearchAfter(HttpContext, searchAfter);

        return organizations;
    }
}