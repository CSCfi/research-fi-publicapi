using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicationController : ControllerBase
    {
        private readonly ILogger<PublicationController> _logger;
        private readonly ISearchService _searchService;

        public PublicationController(
            ILogger<PublicationController> logger,
            ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        /// <summary>
        /// Hae julkaisuja
        /// </summary>
        /// <param name="publicationName">Julkaisun nimi</param>
        /// <returns></returns>
        [HttpGet(Name = "GetPublication")]
        public IEnumerable<Publication> Get(string publicationName)
        {
            return _searchService.Search<Publication>(publicationName);
        }
    }
}