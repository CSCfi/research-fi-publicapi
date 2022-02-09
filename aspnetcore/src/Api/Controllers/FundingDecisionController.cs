using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FundingDecisionController : ControllerBase
    {
        private readonly ILogger<FundingDecisionController> _logger;
        private readonly ISearchService _searchService;

        public FundingDecisionController(
            ILogger<FundingDecisionController> logger,
            ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        /// <summary>
        /// Hae rahoituspäätöksiä
        /// </summary>
        /// <param name="publicationName">Julkaisun nimi</param>
        /// <returns></returns>
        [HttpGet(Name = "GetFundingDecision")]
        public IEnumerable<FundingDecision> Get(string publicationName)
        {
            return _searchService.Search<FundingDecision>(publicationName);
        }
    }
}
