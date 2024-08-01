using AutoMapper;
using Nest;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Service.Models.FundingDecision;
using FundingDecisionApiModel = ResearchFi.FundingDecision.FundingDecision;
using System.Text.Json;

namespace BulkExport;

public class FundingDecisionExporter
{
    private IElasticClient _elasticClient;
    private readonly IMapper _mapper;
    private readonly IndexNameSettings _indexNameSettings;


    public FundingDecisionExporter(IElasticClient elasticClient, IMapper mapper, IndexNameSettings indexNameSettings)
    {
        _elasticClient = elasticClient;
        _mapper = mapper;
        _indexNameSettings = indexNameSettings;
        
    }

    public void Export(JsonSerializerOptions serializerOptions)
    {   
        Console.WriteLine("Funding decision export: started");
        string fundingDecisionIndexName = "";

        var configuredTypesAndIndexNames = _indexNameSettings.GetTypesAndIndexNames();
        foreach (var (indexName, modelType) in configuredTypesAndIndexNames)
        {
            if (modelType.FullName == "CSC.PublicApi.Service.Models.FundingDecision.FundingDecision")
            {
                fundingDecisionIndexName = indexName;
                break;
            }
        }

        if (fundingDecisionIndexName != "")
        {
            // First test using "select all", this needs to be converted to "search after" type search
            var searchResponse = _elasticClient.Search<FundingDecision> (s => s.MatchAll().Index(fundingDecisionIndexName));
            var docs = searchResponse.Documents;

            foreach (var doc in docs)
            {
                FundingDecisionApiModel fundingDecision = _mapper.Map<FundingDecisionApiModel>(doc);

                string jsonString = JsonSerializer.Serialize(fundingDecision, serializerOptions);
                File.WriteAllText("/tmp/funding-decision-test-export.json", jsonString);

                Console.WriteLine(jsonString);
            }
            
            Console.WriteLine($"Funding decision export: complete, export count {searchResponse.Documents.Count}");
        }
        else {
            Console.WriteLine($"Funding decision export: failed, index name not found from configuration");
        }
    }
}
