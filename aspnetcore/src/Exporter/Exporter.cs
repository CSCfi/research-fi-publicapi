using AutoMapper;
using Nest;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Service.Models.FundingCall;
using CSC.PublicApi.Service.Models.FundingDecision;
using CSC.PublicApi.Service.Models.ResearchDataset;
using CSC.PublicApi.Service.Models.Publication;
using FundingCallApiModel = ResearchFi.FundingCall.FundingCall;
using FundingDecisionApiModel = ResearchFi.FundingDecision.FundingDecision;
using ResearchDatasetApiModel = ResearchFi.ResearchDataset.ResearchDataset;
using PublicationApiModel = ResearchFi.Publication.Publication;
using System.Text.Json;

namespace BulkExport;

// Exports documents from Elasticsearch into json files.
public class Exporter
{
    private IElasticClient _elasticClient;
    private readonly IMapper _mapper;
    private readonly IndexNameSettings _indexNameSettings;
    private const int SingleQueryResultLimit = 1000;
    private const string ExportBaseDirectory = "/tmp";

    public Exporter(IElasticClient elasticClient, IMapper mapper, IndexNameSettings indexNameSettings)
    {
        _elasticClient = elasticClient;
        _mapper = mapper;
        _indexNameSettings = indexNameSettings;
        
    }



    private string GetFilename(string modelTypeFullName, long exportFileNumber)
    {
        string exportFileNumberPaddedString = exportFileNumber.ToString("D10");
        string fileTypeString = "";
        switch (modelTypeFullName)
            {
                case "CSC.PublicApi.Service.Models.FundingCall.FundingCall":
                    fileTypeString = "fundingCall";
                    break;
                case "CSC.PublicApi.Service.Models.FundingDecision.FundingDecision":
                    fileTypeString = "fundingDecision";
                    break;
                case "CSC.PublicApi.Service.Models.ResearchDataset.ResearchDataset":
                    fileTypeString = "researchDataset";
                    break;
                case "CSC.PublicApi.Service.Models.Publication.Publication":
                    fileTypeString = "publication";
                    break;
            }
        return $"{ExportBaseDirectory}/{fileTypeString}-{exportFileNumberPaddedString}.json";
    }


    public void Export(JsonSerializerOptions serializerOptions)
    {   
        var configuredTypesAndIndexNames = _indexNameSettings.GetTypesAndIndexNames();

        foreach (var (indexName, modelType) in configuredTypesAndIndexNames)
        {
            long numberOfDocumentsInIndex = 0;
            long numberOfQueryResults = -1;
            long exportFileNumber = 0;
            CountResponse? countResponse = null;

            switch (modelType.FullName)
            {
                /*
                 * FundingCall
                 */
                case "CSC.PublicApi.Service.Models.FundingCall.FundingCall":
                    IHit<FundingCall>? lastHitFundingCall = null;
                    ISearchResponse<FundingCall>? fundingCallSearchResponse = null;

                    // Number of documents in index
                    countResponse = _elasticClient.Count<FundingCall>(c => c.Index(indexName));
                    numberOfDocumentsInIndex = countResponse.Count;
                    Console.WriteLine($"Export: FundingCall: started from index {indexName} containing {numberOfDocumentsInIndex} documents");

                    while (numberOfQueryResults == -1 || numberOfQueryResults >= SingleQueryResultLimit) {
                        // Get batch of documents
                        if (lastHitFundingCall != null) {
                            fundingCallSearchResponse = _elasticClient.Search<FundingCall> (s => s
                                .Index(indexName)
                                .Size(SingleQueryResultLimit)
                                .Query(q => q.MatchAll())
                                .Sort(sort => sort.Ascending(SortSpecialField.DocumentIndexOrder))
                                .SearchAfter(lastHitFundingCall.Sorts)
                            );
                        } else {
                            fundingCallSearchResponse = _elasticClient.Search<FundingCall> (s => s
                                .Index(indexName)
                                .Size(SingleQueryResultLimit)
                                .Query(q => q.MatchAll())
                                .Sort(sort => sort.Ascending(SortSpecialField.DocumentIndexOrder))
                            );
                        }
                        
                        numberOfQueryResults = fundingCallSearchResponse.Documents.Count;
                        if (numberOfQueryResults == 0)
                        {
                            break;
                        } 
                        lastHitFundingCall = fundingCallSearchResponse.Hits.Last();

                        // Process documents: Map from Elastic index model to API model, write into text file
                        foreach (var doc in fundingCallSearchResponse.Documents)
                        {
                            FundingCallApiModel fundingCall = _mapper.Map<FundingCallApiModel>(doc);
                            string jsonString = JsonSerializer.Serialize(fundingCall, serializerOptions);
                            File.WriteAllText(
                                GetFilename(modelType.FullName, ++exportFileNumber),
                                jsonString
                            );
                        }
                        Console.WriteLine($"Export: FundingCall: in progress {exportFileNumber}/{numberOfDocumentsInIndex}");
                    }

                    Console.WriteLine($"Export: FundingCall: complete, exported {exportFileNumber}/{numberOfDocumentsInIndex}");
                    break;



                /*
                 * FundingDecision
                 */ 
                case "CSC.PublicApi.Service.Models.FundingDecision.FundingDecision":
                    IHit<FundingDecision>? lastHitFundingDecision = null;
                    ISearchResponse<FundingDecision>? fundingDecisionSearchResponse = null;

                    // Number of documents in index
                    countResponse = _elasticClient.Count<FundingDecision>(c => c.Index(indexName));
                    numberOfDocumentsInIndex = countResponse.Count;
                    Console.WriteLine($"Export: FundingDecision: started from index {indexName} containing {numberOfDocumentsInIndex} documents");

                    while (numberOfQueryResults == -1 || numberOfQueryResults >= SingleQueryResultLimit) {
                        // Get batch of documents
                        if (lastHitFundingDecision != null) {
                            fundingDecisionSearchResponse = _elasticClient.Search<FundingDecision> (s => s
                                .Index(indexName)
                                .Size(SingleQueryResultLimit)
                                .Query(q => q.MatchAll())
                                .Sort(sort => sort.Ascending(SortSpecialField.DocumentIndexOrder))
                                .SearchAfter(lastHitFundingDecision.Sorts)
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
                        lastHitFundingDecision = fundingDecisionSearchResponse.Hits.Last();

                        // Process documents: Map from Elastic index model to API model, write into text file
                        foreach (var doc in fundingDecisionSearchResponse.Documents)
                        {
                            FundingDecisionApiModel fundingDecision = _mapper.Map<FundingDecisionApiModel>(doc);
                            string jsonString = JsonSerializer.Serialize(fundingDecision, serializerOptions);
                            File.WriteAllText(
                                GetFilename(modelType.FullName, ++exportFileNumber),
                                jsonString
                            );
                        }
                        Console.WriteLine($"Export: FundingDecision: in progress {exportFileNumber}/{numberOfDocumentsInIndex}");
                    }

                    Console.WriteLine($"Export: FundingDecision: complete, exported {exportFileNumber}/{numberOfDocumentsInIndex}");
                    break;



                /*
                 * ResearchDataset
                 */ 
                case "CSC.PublicApi.Service.Models.ResearchDataset.ResearchDataset":
                    IHit<ResearchDataset>? lastHitResearchDataset = null;
                    ISearchResponse<ResearchDataset>? researchDatasetSearchResponse = null;

                    // Number of documents in index
                    countResponse = _elasticClient.Count<ResearchDataset>(c => c.Index(indexName));
                    numberOfDocumentsInIndex = countResponse.Count;
                    Console.WriteLine($"Export: ResearchDataset: started from index {indexName} containing {numberOfDocumentsInIndex} documents");

                    while (numberOfQueryResults == -1 || numberOfQueryResults >= SingleQueryResultLimit) {
                        // Get batch of documents
                        if (lastHitResearchDataset != null) {
                            researchDatasetSearchResponse = _elasticClient.Search<ResearchDataset> (s => s
                                .Index(indexName)
                                .Size(SingleQueryResultLimit)
                                .Query(q => q.MatchAll())
                                .Sort(sort => sort.Ascending(SortSpecialField.DocumentIndexOrder))
                                .SearchAfter(lastHitResearchDataset.Sorts)
                            );
                        } else {
                            researchDatasetSearchResponse = _elasticClient.Search<ResearchDataset> (s => s
                                .Index(indexName)
                                .Size(SingleQueryResultLimit)
                                .Query(q => q.MatchAll())
                                .Sort(sort => sort.Ascending(SortSpecialField.DocumentIndexOrder))
                            );
                        }
                        
                        numberOfQueryResults = researchDatasetSearchResponse.Documents.Count;
                        if (numberOfQueryResults == 0)
                        {
                            break;
                        } 
                        lastHitResearchDataset= researchDatasetSearchResponse.Hits.Last();

                        // Process documents: Map from Elastic index model to API model, write into text file
                        foreach (var doc in researchDatasetSearchResponse.Documents)
                        {
                            ResearchDatasetApiModel researchDataset = _mapper.Map<ResearchDatasetApiModel>(doc);
                            string jsonString = JsonSerializer.Serialize(researchDataset, serializerOptions);
                            File.WriteAllText(
                                GetFilename(modelType.FullName, ++exportFileNumber),
                                jsonString
                            );
                        }
                        Console.WriteLine($"Export: ResearchDataset: in progress {exportFileNumber}/{numberOfDocumentsInIndex}");
                    }
                    break;
    


                /*
                 * Publication
                 */ 
                case "CSC.PublicApi.Service.Models.Publication.Publication":
                    IHit<Publication>? lastHitPublication = null;
                    ISearchResponse<Publication>? publicationSearchResponse = null;

                    // Number of documents in index
                    countResponse = _elasticClient.Count<Publication>(c => c.Index(indexName));
                    numberOfDocumentsInIndex = countResponse.Count;
                    Console.WriteLine($"Export: Publication: started from index {indexName} containing {numberOfDocumentsInIndex} documents");

                    while (numberOfQueryResults == -1 || numberOfQueryResults >= SingleQueryResultLimit) {
                        // Get batch of documents
                        if (lastHitPublication != null) {
                            publicationSearchResponse = _elasticClient.Search<Publication> (s => s
                                .Index(indexName)
                                .Size(SingleQueryResultLimit)
                                .Query(q => q.MatchAll())
                                .Sort(sort => sort.Ascending(SortSpecialField.DocumentIndexOrder))
                                .SearchAfter(lastHitPublication.Sorts)
                            );
                        } else {
                            publicationSearchResponse = _elasticClient.Search<Publication> (s => s
                                .Index(indexName)
                                .Size(SingleQueryResultLimit)
                                .Query(q => q.MatchAll())
                                .Sort(sort => sort.Ascending(SortSpecialField.DocumentIndexOrder))
                            );
                        }
                        
                        numberOfQueryResults = publicationSearchResponse.Documents.Count;
                        if (numberOfQueryResults == 0)
                        {
                            break;
                        } 
                        lastHitPublication= publicationSearchResponse.Hits.Last();

                        // Process documents: Map from Elastic index model to API model, write into text file
                        foreach (var doc in publicationSearchResponse.Documents)
                        {
                            PublicationApiModel publication = _mapper.Map<PublicationApiModel>(doc);
                            string jsonString = JsonSerializer.Serialize(publication, serializerOptions);
                            File.WriteAllText(
                                GetFilename(modelType.FullName, ++exportFileNumber),
                                jsonString
                            );
                        }
                        Console.WriteLine($"Export: Publication: in progress {exportFileNumber}/{numberOfDocumentsInIndex}");
                    }
                    break;

                default:
                    break;
            }
        }

        Console.WriteLine("Export: completed");
    }
}
