using Api.Models;
using Api.Models.Publication;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = ApiPolicies.PublicationSearch)]
    public class PublicationController : ControllerBase
    {
        private readonly ILogger<PublicationController> _logger;
        private readonly ISearchService<PublicationSearchParameters, Publication> _searchService;

        public PublicationController(
            ILogger<PublicationController> logger,
            ISearchService<PublicationSearchParameters, Publication> searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        /// <summary>
        /// Hae julkaisuja
        /// </summary>
        /// <param name="searchParameters">Julkaisun nimi</param>
        /// <param name="paginationParameters"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetPublication")]
        public async Task<IEnumerable<Publication>> Get([FromQuery] PublicationSearchParameters searchParameters, [FromQuery]PaginationParameters paginationParameters)
        {
            var result = await _searchService.Search(searchParameters, paginationParameters.PageNumber, paginationParameters.PageSize);
            ResponseHelper.AddPaginationResponseHeaders(HttpContext, result);

            return result;
        }
    }
}