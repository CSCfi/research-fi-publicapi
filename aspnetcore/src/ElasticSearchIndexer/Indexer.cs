using Api.DataAccess;
using Api.Maps;
using Api.Models.Entities;
using Api.Models.FundingCall;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace ElasticSearchIndexer
{
    public class Indexer
    {
        private readonly ILogger<Indexer> _logger;
        private readonly IElasticSearchIndexService _indexService;
        private readonly IMapper<DimCallProgramme, FundingCall> _fundingCallMapper;
        private readonly IFundingCallRepository _fundingCallRepository;

        public Indexer(
            ILogger<Indexer> logger,
            IElasticSearchIndexService indexService,
            IMapper<DimCallProgramme, FundingCall> fundingCallMapper,
            IFundingCallRepository fundingCallRepository)
        {
            _logger = logger;
            _indexService = indexService;
            _fundingCallMapper = fundingCallMapper;
            _fundingCallRepository = fundingCallRepository;
        }

        public async Task Start()
        {
            var indexName = "api-dev-funding-call";

            _logger.LogInformation("Getting entities from the database.");

            // Get entities from DB
            var dbFundingCalls = _fundingCallRepository.GetAllAsync();

            // Map to api model
            var apiFundingCalls = await dbFundingCalls.Select(entity => _fundingCallMapper.Map(entity)).ToListAsync();
            _logger.LogInformation("Mapped {Count} entities to api models.", apiFundingCalls.Count);

            // Index api models
            _logger.LogInformation("Indexing {indexName}..", indexName);
            await _indexService.IndexAsync(indexName, apiFundingCalls);

            _logger.LogInformation("Indexing done.");
        }
    }
}
