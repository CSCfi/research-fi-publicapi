using Api.Models;
using Api.Models.Organization;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersion)]
    [Route("v{version:apiVersion}/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private const string ApiVersion = "1.0";

        private readonly ILogger<OrganizationController> _logger;
        private readonly ISearchService<OrganizationSearchParameters, Organization> _searchService;

        public OrganizationController(
            ILogger<OrganizationController> logger,
            ISearchService<OrganizationSearchParameters, Organization> searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        /// <summary>
        /// Hae organisaatioita
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="paginationParameters"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetOrganization")]
        [MapToApiVersion(ApiVersion)]
        [Authorize(Policy = ApiPolicies.Organization.Search)]
        public async Task<IEnumerable<Organization>> Get([FromQuery] OrganizationSearchParameters searchParameters, [FromQuery]PaginationParameters paginationParameters)
        {
            var result = await _searchService.Search(searchParameters, paginationParameters.PageNumber, paginationParameters.PageSize);
            ResponseHelper.AddPaginationResponseHeaders(HttpContext, result);

            return result;
        }
    }
}
