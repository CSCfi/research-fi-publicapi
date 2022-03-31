using Api.Models.FundingDecision;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersion)]
    [Route("v{version:apiVersion}/[controller]")]
    
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
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetFundingDecision")]
        [MapToApiVersion(ApiVersion)]
        [Authorize(Policy = ApiPolicies.FundingDecision.Search)]
        public IEnumerable<FundingDecision> Get([FromQuery] FundingDecisionSearchParameters searchParameters)
        {
            return _searchService.Search(searchParameters);
        }
    }
}
