using Api.DataAccess.Repositories;
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
        private readonly IConfiguration _configuration;
        private readonly Stopwatch _stopWatch = new();
        private readonly IIndexRepository<FundingCall> _fundingCallRepository;
        private readonly IIndexRepository<FundingDecision> _fundingDecisionRepository;
        private readonly IIndexRepository<Api.Models.Infrastructure.Infrastructure> _infrastructureRepository;

        public Indexer(
            ILogger<Indexer> logger,
            IElasticSearchIndexService indexService,
            IIndexRepository<FundingCall> fundingCallRepository,
            IIndexRepository<FundingDecision> fundingDecisionRepository,
            IIndexRepository<Api.Models.Infrastructure.Infrastructure> infrastructureRepository,
            IConfiguration configuration
            )
        {
            _logger = logger;
            _indexService = indexService;
            _fundingCallRepository = fundingCallRepository;
            _fundingDecisionRepository = fundingDecisionRepository;
            _infrastructureRepository = infrastructureRepository;
            _configuration = configuration;
        }

        public async Task Start()
        {
            _stopWatch.Start();
            _logger.LogInformation("Starting indexing.. {stopWatch}", _stopWatch.Elapsed);

            var elasticLog = $"Using ElasticSearch '{_configuration["ELASTICSEARCH:URL"]}'";
            elasticLog += _configuration["ELASTICSEARCH:USERNAME"] != null || _configuration["ELASTICSEARCH:PASSWORD"] != null
                ? " with basic authentication."
                : ".";
            _logger.LogInformation(elasticLog);

            await IndexEntities("api-dev-funding-call", _fundingCallRepository);
            await IndexEntities("api-dev-funding-decision", _fundingDecisionRepository);
            await IndexEntities("api-dev-infrastructure", _infrastructureRepository);

            _logger.LogInformation("All indexing done. {stopWatch}", _stopWatch.Elapsed);
            _stopWatch.Stop();
        }

        private async Task IndexEntities<TIndexModel>(
            string indexName,
            IIndexRepository<TIndexModel> repository
        ) where TIndexModel : class
        {
            _logger.LogInformation("Getting '{entityType}' entities from the database. {stopWatch}", typeof(TIndexModel).Name, _stopWatch.Elapsed);

            var indexModels = await repository.GetAllAsync().ToListAsync();
            _logger.LogInformation("Got {count} '{entityType}' entities from the database. {stopWatch}", indexModels.Count, typeof(TIndexModel).Name, _stopWatch.Elapsed);

            _logger.LogInformation("Indexing '{indexName}'..", indexName);

            await _indexService.IndexAsync(indexName, indexModels);
            _logger.LogInformation("Index '{indexName}' created. {stopWatch}", indexName, _stopWatch.Elapsed);
        }
    }
}
