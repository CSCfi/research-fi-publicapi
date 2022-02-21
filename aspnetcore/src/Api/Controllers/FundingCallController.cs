using Api.DatabaseContext;
using Api.Models.Entities;
using Api.Models.FundingCall;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = ApiPolicies.FundingCallSearch)]
    public class FundingCallController : ControllerBase
    {

        private readonly ILogger<FundingCallController> _logger;
        private readonly ISearchService<FundingCallSearchParameters,FundingCall> _searchService;
        private readonly ApiDbContext _dbContext;

        public FundingCallController(
            ILogger<FundingCallController> logger,
            ISearchService<FundingCallSearchParameters,FundingCall> searchService,
            ApiDbContext dbContext)
        {
            _logger = logger;
            _searchService = searchService;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Hae rahoitushakuja
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetFundingCall")]
        public IEnumerable<FundingCall> Get([FromQuery]FundingCallSearchParameters searchParameters)
        {
            return _searchService.Search(searchParameters);
        }

        [HttpPost(Name = "PostFundingCall")]
        public async Task Post(FundingCall fundingCall)
        {
            await _dbContext
                .Set<DimCallProgramme>()
                .AddAsync(new DimCallProgramme
                {
                    NameFi = "test fi",
                    SourceId = "api",
                    DimRegisteredDataSourceId = -1
                });
            await _dbContext.SaveChangesAsync();
        }
    }
}
