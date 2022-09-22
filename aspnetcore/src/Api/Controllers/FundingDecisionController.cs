using Api.Models;
using Api.Models.FundingDecision;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersion)]
    [Route("v{version:apiVersion}/funding-decisions")]
    
    public class FundingDecisionController : ControllerBase
    {
        private const string ApiVersion = "1.0";
        private readonly ILogger<FundingDecisionController> _logger;
        private readonly ISearchService<FundingDecisionSearchParameters, FundingDecision> _searchService;

        public FundingDecisionController(
            ILogger<FundingDecisionController> logger,
            ISearchService<FundingDecisionSearchParameters,FundingDecision> searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        /// <summary>
        /// Hae rahoituspäätöksiä
        /// </summary>
        /// <param name="searchParameters">Query parameters for filtering the results.</param>
        /// <param name="paginationParameters">Query parameters for pagination of the results.</param>
        /// <returns></returns>
        [HttpGet(Name = "GetFundingDecision")]
        [MapToApiVersion(ApiVersion)]
        [Authorize(Policy = ApiPolicies.FundingDecision.Search)]
        public async Task<SearchResult<FundingDecision>> Get([FromQuery] FundingDecisionSearchParameters searchParameters, [FromQuery] PaginationParameters paginationParameters)
        {
            var result = await _searchService.Search(searchParameters, paginationParameters.PageNumber, paginationParameters.PageSize);
            ResponseHelper.AddPaginationResponseHeaders(HttpContext, result);

            return result;
        }
    }
}
