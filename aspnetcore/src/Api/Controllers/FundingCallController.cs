using Api.Models.FundingCall;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = ApiPolicies.FundingCallSearch)]
    public class FundingCallController : ControllerBase
    {

        private readonly ILogger<FundingCallController> _logger;
        private readonly ISearchService<FundingCallSearchParameters,FundingCall> _searchService;

        public FundingCallController(
            ILogger<FundingCallController> logger,
            ISearchService<FundingCallSearchParameters,FundingCall> searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        /// <summary>
        /// Hae rahoitushakuja
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetFundingCall")]
        public IEnumerable<FundingCall> Get([FromQuery]FundingCallSearchParameters searchParameters)
        {
            return _searchService.Search(searchParameters);
        }
    }
}
