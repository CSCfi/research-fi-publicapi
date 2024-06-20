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
        Console.WriteLine("Start exporting funding decisions");
        //string type = "CSC.PublicApi.Service.Models.FundingDecision.FundingDecision";
        //var configuredTypesAndIndexNames = _indexNameSettings.GetTypesAndIndexNames();

        // First test using "select all", this needs to be converted to "search after" type search
        var searchResponse = _elasticClient.Search<FundingDecision> (s => s.MatchAll().Index("api-dev-funding-decision"));
        var docs = searchResponse.Documents;

        foreach (var doc in docs)
        {
            FundingDecisionApiModel fundingDecision = _mapper.Map<FundingDecisionApiModel>(doc);

            string jsonString = JsonSerializer.Serialize(fundingDecision, serializerOptions);
            File.WriteAllText("/tmp/funding-decision-test-export.json", jsonString);

            Console.WriteLine(jsonString);
        }
        
        Console.WriteLine($"Done exporting funding calls: {searchResponse.Documents.Count}");
    }
}
