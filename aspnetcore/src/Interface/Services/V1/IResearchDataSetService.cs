/*
 * API version 1.0
 */
using CSC.PublicApi.ElasticService;
using ResearchFi.Query;
using ResearchFi.ResearchDataset.V1;

namespace CSC.PublicApi.Interface.Services.V1;

public interface IResearchDatasetService
{
    Task<(IEnumerable<ResearchDataset>, SearchResult)> GetResearchDatasets(GetResearchDatasetsQueryParameters researchDatasetsQueryParameters, PaginationQueryParameters paginationQueryParameters);
    Task<(IEnumerable<ResearchDataset>, SearchAfterResult)> GetResearchDatasetsSearchAfter(GetResearchDatasetsQueryParameters researchDatasetsQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters);
}