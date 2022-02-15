using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class FundingDecisionController : ControllerBase
    {
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
        public IEnumerable<FundingDecision> Get([FromQuery] FundingDecisionSearchParameters searchParameters)
        {
            return _searchService.Search(searchParameters);
        }
    }
}
