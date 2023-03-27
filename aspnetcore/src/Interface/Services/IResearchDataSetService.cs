using CSC.PublicApi.ElasticService;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.ResearchDataset;

namespace CSC.PublicApi.Interface.Services;

public interface IResearchDatasetService
{
    Task<(IEnumerable<ResearchDataset>, SearchResult)> GetResearchDatasets(GetResearchDatasetsQueryParameters queryParameters);
}