using Api.Models;
using Api.Models.Infrastructure;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersion)]
    [Route("v{version:apiVersion}/infrastructures")]
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
        /// Search Infrastructures
        /// </summary>
        /// <param name="searchParameters">Query parameters for filtering the results.</param>
        /// <param name="paginationParameters">Query parameters for pagination of the results.</param>
        /// <returns></returns>
        [HttpGet(Name = "GetInfratructure")]
        [MapToApiVersion(ApiVersion)]
        [Authorize(Policy = ApiPolicies.Infrastructure.Search)]
        public async Task<SearchResult<Infrastructure>> Get([FromQuery] InfrastructureSearchParameters searchParameters, [FromQuery] PaginationParameters paginationParameters)
        {
            var result = await _searchService.Search(searchParameters, paginationParameters.PageNumber, paginationParameters.PageSize);
            ResponseHelper.AddPaginationResponseHeaders(HttpContext, result);

            return result;
        }
    }
}
