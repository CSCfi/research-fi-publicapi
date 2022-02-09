using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FundingCallController : ControllerBase
    {

        private readonly ILogger<FundingCallController> _logger;
        private readonly ISearchService<FundingCall> _searchService;

        public FundingCallController(
            ILogger<FundingCallController> logger,
            ISearchService<FundingCall> searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        /// <summary>
        /// Hae rahoitushakuja
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetFundingCall")]
        public IEnumerable<FundingCall> Get(string searchText)
        {
            return _searchService.Search(searchText);
        }
    }
}
