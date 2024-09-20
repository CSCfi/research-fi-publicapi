using CSC.PublicApi.ElasticService;
using ResearchFi.Query;
using ResearchFi.ResearchDataset;

namespace CSC.PublicApi.Interface.Services;

public interface IResearchDatasetService
{
    Task<(IEnumerable<ResearchDataset>, SearchResult)> GetResearchDatasets(GetResearchDatasetsQueryParameters researchDatasetsQueryParameters, PaginationQueryParameters paginationQueryParameters);
    Task<(IEnumerable<ResearchDataset>, long? searchAfter)> GetResearchDatasetsSearchAfter(GetResearchDatasetsQueryParameters researchDatasetsQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters);
}