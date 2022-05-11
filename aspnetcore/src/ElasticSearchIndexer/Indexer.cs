using Api.DataAccess.Repositories;
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
        private readonly IIndexRepository<Api.Models.FundingCall.FundingCall> _fundingCallRepository;
        private readonly IIndexRepository<Api.Models.FundingDecision.FundingDecision> _fundingDecisionRepository;
        private readonly IIndexRepository<Api.Models.Infrastructure.Infrastructure> _infrastructureRepository;
        private readonly IIndexRepository<Api.Models.Organization.Organization> _organizationRepository;
        private readonly IIndexRepository<Api.Models.ResearchDataset.ResearchDataset> _researchDatasetRepository;

        public Indexer(
            ILogger<Indexer> logger,
            IElasticSearchIndexService indexService,
            IIndexRepository<Api.Models.FundingCall.FundingCall> fundingCallRepository,
            IIndexRepository<Api.Models.FundingDecision.FundingDecision> fundingDecisionRepository,
            IIndexRepository<Api.Models.Infrastructure.Infrastructure> infrastructureRepository,
            IIndexRepository<Api.Models.Organization.Organization> organizationRepository,
            IIndexRepository<Api.Models.ResearchDataset.ResearchDataset> researchDatasetRepository,
            IConfiguration configuration
            )
        {
            _logger = logger;
            _indexService = indexService;
            _fundingCallRepository = fundingCallRepository;
            _fundingDecisionRepository = fundingDecisionRepository;
            _infrastructureRepository = infrastructureRepository;
            _organizationRepository = organizationRepository;
            _researchDatasetRepository = researchDatasetRepository;
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
            await IndexEntities("api-dev-organization", _organizationRepository);
            await IndexEntities("api-dev-researchdataset", _researchDatasetRepository);

            _logger.LogInformation("All indexing done. {stopWatch}", _stopWatch.Elapsed);
            _stopWatch.Stop();
        }

        private async Task IndexEntities<TIndexModel>(
            string indexName,
            IIndexRepository<TIndexModel> repository
        ) where TIndexModel : class
        {
            try
            {

                _logger.LogInformation("Getting '{entityType}' entities from the database. {stopWatch}", typeof(TIndexModel).Name, _stopWatch.Elapsed);

                var indexModels = await repository.GetAllAsync().ToListAsync();
                _logger.LogInformation("Got {count} '{entityType}' entities from the database. {stopWatch}", indexModels.Count, typeof(TIndexModel).Name, _stopWatch.Elapsed);

                _logger.LogInformation("Indexing '{indexName}'..", indexName);

                await _indexService.IndexAsync(indexName, indexModels);
                _logger.LogInformation("Index '{indexName}' created. {stopWatch}", indexName, _stopWatch.Elapsed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while indexing '{indexName}' {stopWatch},", indexName, _stopWatch.Elapsed);
            }
        }
    }
}
