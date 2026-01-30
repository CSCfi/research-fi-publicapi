using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.Query;

namespace CSC.PublicApi.Interface.Services;

public class InfrastructureServiceService : IInfrastructureServiceService
{
    private readonly IMapper _mapper;
    private readonly ISearchService<InfrastructureServiceSearchParameters, Service.Models.Infrastructure.Service> _searchService;

    public InfrastructureServiceService(
        IMapper mapper,
        ISearchService<InfrastructureServiceSearchParameters,
        Service.Models.Infrastructure.Service> searchService)
    {
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task<(IEnumerable<ResearchFi.Infrastructure.Service>, SearchResult)> GetInfrastructureServices(GetInfrastructureServicesQueryParameters infrastructureServicesQueryParameters, PaginationQueryParameters paginationQueryParameters)
    {
        var searchParameters = _mapper.Map<InfrastructureServiceSearchParameters>(infrastructureServicesQueryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, paginationQueryParameters.PageNumber, paginationQueryParameters.PageSize);

        // If query parameters contain ExcludeInfrastructures=true, omit IsPartOf property
        if (infrastructureServicesQueryParameters.ExcludeInfrastructures == true)
        {
            foreach (var infrastructure in result)
            {
                infrastructure.IsPartOfInfrastructure = null;
            }
        }

        return (_mapper.Map<IEnumerable<ResearchFi.Infrastructure.Service>>(result), searchResult);
    }


    public async Task<(IEnumerable<ResearchFi.Infrastructure.Service>, SearchAfterResult)> GetInfrastructureServicesSearchAfter(GetInfrastructureServicesQueryParameters infrastructureServicesQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var searchParameters = _mapper.Map<InfrastructureServiceSearchParameters>(infrastructureServicesQueryParameters);

        var (result, searchAfterResult) = await _searchService.SearchAfter(searchParameters, searchAfterQueryParameters.PageSize, searchAfterQueryParameters.NextPageToken);

        // If query parameters contain ExcludeInfrastructures=true, omit IsPartOf property
        if (infrastructureServicesQueryParameters.ExcludeInfrastructures == true)
        {
            foreach (var infrastructure in result)
            {
                infrastructure.IsPartOfInfrastructure = null;
            }
        }

        return (_mapper.Map<IEnumerable<ResearchFi.Infrastructure.Service>>(result), searchAfterResult);
    }

    public async Task<ResearchFi.Infrastructure.Service?> GetInfrastructureService(string urn)
    {
        var result = await _searchService.GetSingle(urn);

        return _mapper.Map<ResearchFi.Infrastructure.Service>(result);
    }
}