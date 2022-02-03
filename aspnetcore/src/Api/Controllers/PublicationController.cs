using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicationController : ControllerBase
    {
        private readonly ILogger<PublicationController> _logger;

        public PublicationController(ILogger<PublicationController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetPublication")]
        public IEnumerable<Publication> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Publication
            {
                Name = $"Julkaisu {index}"
            })
            .ToArray();
        }
    }
}