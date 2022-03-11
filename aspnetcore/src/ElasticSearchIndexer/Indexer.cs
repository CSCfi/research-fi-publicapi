using Api.DataAccess.Repositories;
using Api.Maps;
using Api.Models.Entities;
using Api.Models.FundingCall;
using Api.Models.FundingDecision;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ElasticSearchIndexer
{
    public class Indexer
    {
        private readonly ILogger<Indexer> _logger;
        private readonly IElasticSearchIndexService _indexService;
        private readonly IMapper<DimCallProgramme, FundingCall> _fundingCallMapper;
        private readonly IFundingCallRepository _fundingCallRepository;
        private readonly IMapper<DimFundingDecision, FundingDecision> _fundingDecisionMapper;
        private readonly IFundingDecisionRepository _fundingDecisionRepository;
        private readonly IConfiguration _configuration;
        private readonly Stopwatch _stopWatch = new();

        public Indexer(
            ILogger<Indexer> logger,
            IElasticSearchIndexService indexService,
            IMapper<DimCallProgramme, FundingCall> fundingCallMapper,
            IFundingCallRepository fundingCallRepository,
            IMapper<DimFundingDecision, FundingDecision> fundingDecisionMapper,
            IFundingDecisionRepository fundingDecisionRepository,
            IConfiguration configuration)
        {
            _logger = logger;
            _indexService = indexService;
            _fundingCallMapper = fundingCallMapper;
            _fundingCallRepository = fundingCallRepository;
            _fundingDecisionMapper = fundingDecisionMapper;
            _fundingDecisionRepository = fundingDecisionRepository;
            _configuration = configuration;
        }

        public async Task Start()
        {
            _stopWatch.Start();
            _logger.LogInformation("Starting indexing.. {stopWatch}", _stopWatch.Elapsed);

            await IndexEntities("api-dev-funding-call", _fundingCallRepository, _fundingCallMapper);
            await IndexEntities("api-dev-funding-decision", _fundingDecisionRepository, _fundingDecisionMapper);

            _logger.LogInformation("All indexing done. {stopWatch}", _stopWatch.Elapsed);
            _stopWatch.Stop();
        }

        private async Task IndexEntities<TEntity, TIndexModel>(
            string indexName,
            IGenericRepository<TEntity> repository,
            IMapper<TEntity, TIndexModel> mapper
        ) where TEntity : class where TIndexModel : class
        {
            _logger.LogInformation("Getting '{entityType}' entities from the database. {stopWatch}", typeof(TEntity).Name, _stopWatch.Elapsed);
            var entities = repository.GetAllAsync();

            var indexModels = await entities.Select(entity => mapper.Map(entity)).ToListAsync();
            _logger.LogInformation("Mapped {Count} '{entityType}' entities to '{indexModelType}'. {stopWatch}", indexModels.Count, typeof(TEntity).Name, typeof(TIndexModel).Name, _stopWatch.Elapsed);

            var elasticLog = $"Using ElasticSearch '{_configuration["ELASTICSEARCH:URL"]}'";
            elasticLog += _configuration["ELASTICSEARCH:USERNAME"] != null || _configuration["ELASTICSEARCH:PASSWORD"] != null
                ? " with basic authentication."
                : ".";
            _logger.LogInformation(elasticLog);
            _logger.LogInformation("Indexing '{indexName}'..", indexName);

            await _indexService.IndexAsync(indexName, indexModels);
            _logger.LogInformation("Index '{indexName}' created. {stopWatch}", indexName, _stopWatch.Elapsed);
        }
    }
}
