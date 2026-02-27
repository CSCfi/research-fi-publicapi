/*
 * API version 2.0
 */
using CSC.PublicApi.ElasticService;
using ResearchFi.Query;
using ResearchFi.ResearchDataset.V2;

namespace CSC.PublicApi.Interface.Services.V2;

public interface IResearchDatasetService
{
    Task<(IEnumerable<ResearchDataset>, SearchResult)> GetResearchDatasets(GetResearchDatasetsQueryParameters researchDatasetsQueryParameters, PaginationQueryParameters paginationQueryParameters);
    Task<(IEnumerable<ResearchDataset>, SearchAfterResult)> GetResearchDatasetsSearchAfter(GetResearchDatasetsQueryParameters researchDatasetsQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters);
}