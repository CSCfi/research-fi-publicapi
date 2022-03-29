using Api.DataAccess.Repositories;
using Api.Maps;
using Api.Models.Entities;
using Api.Models.FundingCall;
using Api.Models.FundingDecision;
using Api.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        //private readonly IMapper<DimCallProgramme, FundingCall> _fundingCallMapper;
        //private readonly IFundingCallRepository _fundingCallRepository;
        //private readonly IMapper<DimFundingDecision, FundingDecision> _fundingDecisionMapper;
        //private readonly IFundingDecisionRepository _fundingDecisionRepository;
        private readonly IConfiguration _configuration;
        private readonly Stopwatch _stopWatch = new();
        private readonly IIndexRepository<FundingCall> _fundingCallRepository;
        private readonly IIndexRepository<FundingDecision> _fundingDecisionRepository;
        //private readonly IGenericRepository<DimFundingDecision> _fundingDecisionRepository;

        public Indexer(
            ILogger<Indexer> logger,
            IElasticSearchIndexService indexService,
            //IMapper<DimCallProgramme, FundingCall> fundingCallMapper,
            //IFundingCallRepository fundingCallRepository,
            //IMapper<DimFundingDecision, FundingDecision> fundingDecisionMapper,
            //IFundingDecisionRepository fundingDecisionRepository,
            //IGenericRepository<DimFundingDecision> fundingDecisionRepository,
            IIndexRepository<FundingCall> fundingCallRepository,
            IIndexRepository<FundingDecision> fundingDecisionRepository,
            IConfiguration configuration
            )
        {
            _logger = logger;
            _indexService = indexService;
            //_fundingCallMapper = fundingCallMapper;
            _fundingCallRepository = fundingCallRepository;
            //_fundingDecisionMapper = fundingDecisionMapper;
            //_fundingDecisionRepository = fundingDecisionRepository;
            _fundingDecisionRepository = fundingDecisionRepository;
            _configuration = configuration;
        }

        public async Task Start()
        {
            _stopWatch.Start();
            _logger.LogInformation("Starting indexing.. {stopWatch}", _stopWatch.Elapsed);

            //await IndexEntities("api-dev-funding-call", _fundingCallRepository, _fundingCallMapper);
            //await IndexEntities("api-dev-funding-decision", _fundingDecisionRepository, _fundingDecisionMapper);

            //var x = await _fundingDecisionRepository.GetAllAsync().ToListAsync();

            await IndexEntities("api-dev-funding-call", _fundingCallRepository);
            await IndexEntities("api-dev-funding-decision", _fundingDecisionRepository);

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
