using Api.Models.Infrastructure;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersion)]
    [Route("v{version:apiVersion}/[controller]")]
    public class InfrastructureController : ControllerBase
    {
        private const string ApiVersion = "1.0";

        private readonly ILogger<InfrastructureController> _logger;
        private readonly ISearchService<InfrastructureSearchParameters, Infrastructure> _searchService;

        public InfrastructureController(
            ILogger<InfrastructureController> logger,
            ISearchService<InfrastructureSearchParameters, Infrastructure> searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        /// <summary>
        /// Hae infrastruktuureja
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetInfratructure")]
        [MapToApiVersion(ApiVersion)]
        [Authorize(Policy = ApiPolicies.Infrastructure.Search)]
        public IEnumerable<Models.Infrastructure.Infrastructure> Get([FromQuery] InfrastructureSearchParameters searchParameters)
        {
            return _searchService.Search(searchParameters);
        }
    }
}
