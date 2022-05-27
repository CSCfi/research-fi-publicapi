using Api.DataAccess;
using Api.Models.Entities;
using Api.Models.FundingCall;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FundingCall = Api.Models.FundingCall.FundingCall;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersion)]
    [Route("v{version:apiVersion}/[controller]")]
    public class FundingCallController : ControllerBase
    {
        private const string ApiVersion = "1.0";

        private readonly ILogger<FundingCallController> _logger;
        private readonly ISearchService<FundingCallSearchParameters, FundingCall> _searchService;
        private readonly IUnitOfWork _unitOfWork;

        public FundingCallController(
            ILogger<FundingCallController> logger,
            ISearchService<FundingCallSearchParameters, FundingCall> searchService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _searchService = searchService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Hae rahoitushakuja
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetFundingCall")]
        [MapToApiVersion(ApiVersion)]
        [Authorize(Policy = ApiPolicies.FundingCall.Search)]
        public async Task<IEnumerable<FundingCall>> Get([FromQuery]FundingCallSearchParameters searchParameters)
        {
            return await _searchService.Search(searchParameters);
        }

        /// <summary>
        /// Lisää uusi rahoitushaku
        /// </summary>
        /// <param name="fundingCall"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostFundingCall")]
        [MapToApiVersion(ApiVersion)]
        [Authorize(Policy = ApiPolicies.FundingCall.Add)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task Post(FundingCall fundingCall)
        {
            // TODO: only NameFi mapped to entity. Not using final models yet.
            await _unitOfWork
                .FundingCalls
                .AddAsync(new DimCallProgramme
                {
                    NameFi = fundingCall.NameFi,
                    SourceId = "api",
                    DimRegisteredDataSourceId = -1
                });
            await _unitOfWork.CompleteAsync();
        }
    }
}
