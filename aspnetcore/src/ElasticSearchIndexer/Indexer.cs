using Api.DatabaseContext;
using Api.Maps;
using Api.Models.Entities;
using Api.Models.FundingCall;
using Api.Services;
using Microsoft.Extensions.Logging;

namespace ElasticSearchIndexer
{
    public class Indexer
    {
        private readonly ILogger<Indexer> _logger;
        private readonly IElasticSearchIndexService _indexService;
        private readonly ApiDbContext _dbContext;
        private readonly IMapper<DimCallProgramme, FundingCall> _fundingCallMapper;

        public Indexer(
            ILogger<Indexer> logger,
            IElasticSearchIndexService indexService,
            ApiDbContext dbContext,
            IMapper<DimCallProgramme, FundingCall> fundingCallMapper)
        {
            _logger = logger;
            _indexService = indexService;
            _dbContext = dbContext;
            _fundingCallMapper = fundingCallMapper;
        }

        public async Task Start()
        {

            var indexName = "api-dev-funding-call";

            _logger.LogInformation("Getting entities from the database.");

            // Get entities from DB
            var dbFundingCalls = _dbContext.Set<DimCallProgramme>().ToList();

            _logger.LogInformation("Got {Count} entities from the database.", dbFundingCalls.Count);

            // Map to api model
            var apiFundingCalls = dbFundingCalls.Select(fc => _fundingCallMapper.Map(fc)).ToList();

            _logger.LogInformation("Indexing {indexName}..", indexName);

            // Index api models
            await _indexService.IndexAsync(indexName, apiFundingCalls);
        }
    }
}
