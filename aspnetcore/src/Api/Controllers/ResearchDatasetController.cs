using Api.Models.ResearchDataset;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersion)]
    [Route("v{version:apiVersion}/[controller]")]
    public class ResearchDatasetController : ControllerBase
    {
        private const string ApiVersion = "1.0";

        private readonly ILogger<ResearchDatasetController> _logger;
        private readonly ISearchService<ResearchDatasetSearchParameters, ResearchDataset> _searchService;

        public ResearchDatasetController(
            ILogger<ResearchDatasetController> logger,
            ISearchService<ResearchDatasetSearchParameters, ResearchDataset> searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        /// <summary>
        /// Hae aineistoja
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetResearchDataset")]
        [MapToApiVersion(ApiVersion)]
        [Authorize(Policy = ApiPolicies.ResearchDataset.Search)]
        public async Task<IEnumerable<ResearchDataset>> Get([FromQuery] ResearchDatasetSearchParameters searchParameters)
        {
            return await _searchService.Search(searchParameters);
        }
    }
}
