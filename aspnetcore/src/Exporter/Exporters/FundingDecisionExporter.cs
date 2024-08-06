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
    private const int SingleQueryResultLimit = 250;

    public FundingDecisionExporter(IElasticClient elasticClient, IMapper mapper, IndexNameSettings indexNameSettings)
    {
        _elasticClient = elasticClient;
        _mapper = mapper;
        _indexNameSettings = indexNameSettings;
        
    }

    public void Export(JsonSerializerOptions serializerOptions)
    {   
        var configuredTypesAndIndexNames = _indexNameSettings.GetTypesAndIndexNames();
        foreach (var (indexName, modelType) in configuredTypesAndIndexNames)
        {
            switch (modelType.FullName)
            {
                case "CSC.PublicApi.Service.Models.FundingDecision.FundingDecision":
                    long numberOfDocumentsInIndex = 0;
                    long numberOfQueryResults = -1;
                    long exportFileNumber = 0;
                    IHit<FundingDecision>? lastHit = null;
                    ISearchResponse<FundingDecision>? fundingDecisionSearchResponse = null;

                    // Number of documents in index
                    var countResponse = _elasticClient.Count<FundingDecision>(c => c.Index(indexName));
                    numberOfDocumentsInIndex = countResponse.Count;
                    Console.WriteLine($"Export: FundingDecision: started from index {indexName} containing {numberOfDocumentsInIndex} documents");

                    while (numberOfQueryResults == -1 || numberOfQueryResults >= SingleQueryResultLimit) {
                        // Get batch of documents
                        if (lastHit != null) {
                            fundingDecisionSearchResponse = _elasticClient.Search<FundingDecision> (s => s
                                .Index(indexName)
                                .Size(SingleQueryResultLimit)
                                .Query(q => q.MatchAll())
                                .Sort(sort => sort.Ascending(SortSpecialField.DocumentIndexOrder))
                                .SearchAfter(lastHit.Sorts)
                            );
                        } else {
                            fundingDecisionSearchResponse = _elasticClient.Search<FundingDecision> (s => s
                                .Index(indexName)
                                .Size(SingleQueryResultLimit)
                                .Query(q => q.MatchAll())
                                .Sort(sort => sort.Ascending(SortSpecialField.DocumentIndexOrder))
                            );
                        }
                        
                        numberOfQueryResults = fundingDecisionSearchResponse.Documents.Count;
                        if (numberOfQueryResults == 0)
                        {
                            break;
                        } 
                        lastHit = fundingDecisionSearchResponse.Hits.Last();

                        // Process documents: Map from Elastic index model to API model, write into text file
                        foreach (var doc in fundingDecisionSearchResponse.Documents)
                        {
                            ++exportFileNumber;
                            FundingDecisionApiModel fundingDecision = _mapper.Map<FundingDecisionApiModel>(doc);
                            string jsonString = JsonSerializer.Serialize(fundingDecision, serializerOptions);
                            //File.WriteAllText("/tmp/funding-call-test-export.json", jsonString);
                        }
                        Console.WriteLine($"Export: FundingDecision: in progress {exportFileNumber}/{numberOfDocumentsInIndex}");
                    }

                    Console.WriteLine($"Export: FundingDecision: complete, exported {exportFileNumber}/{numberOfDocumentsInIndex}");
                    break;

                case "CSC.PublicApi.Service.Models.FundingCall.FundingCall":
                    break;

                case "CSC.PublicApi.Service.Models.Infrastructure.Infrastructure":
                    Console.WriteLine($"Export: Infrastucture: TODO");
                    break;

                case "CSC.PublicApi.Service.Models.Organization.Organization":
                    Console.WriteLine($"Export: Organization: TODO");
                    break;

                case "CSC.PublicApi.Service.Models.ResearchDataset.ResearchDataset":
                    Console.WriteLine($"Export: Research dataset: TODO");
                    break;
    
                case "CSC.PublicApi.Service.Models.Publication.Publication":
                    Console.WriteLine($"Export: Publication: TODO");
                    break;

                default:
                    break;
            }
        }

        Console.WriteLine("Export: completed");
    }
}
