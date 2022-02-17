using Api.Models;
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
        /// <returns></returns>
        [HttpGet(Name = "GetPublication")]
        public IEnumerable<Publication> Get([FromQuery] PublicationSearchParameters searchParameters)
        {
            return _searchService.Search(searchParameters);
        }
    }
}