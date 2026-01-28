using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.Query;
using Infrastructure = ResearchFi.Infrastructure.Infrastructure;

namespace CSC.PublicApi.Interface.Services;

public class InfrastructureService : IInfrastructureService
{
    private readonly IMapper _mapper;
    private readonly ISearchService<InfrastructureSearchParameters, Service.Models.Infrastructure.Infrastructure> _searchService;

    public InfrastructureService(
        IMapper mapper,
        ISearchService<InfrastructureSearchParameters,
        Service.Models.Infrastructure.Infrastructure> searchService)
    {
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task<(IEnumerable<Infrastructure>, SearchResult)> GetInfrastructures(GetInfrastructuresQueryParameters infrastructuresQueryParameters, PaginationQueryParameters paginationQueryParameters)
    {
        var searchParameters = _mapper.Map<InfrastructureSearchParameters>(infrastructuresQueryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, paginationQueryParameters.PageNumber, paginationQueryParameters.PageSize);

        // If query parameters contain ExcludeServices=true, omit IsComposedOf property
        if (infrastructuresQueryParameters.ExcludeServices == true)
        {
            foreach (var infrastructure in result)
            {
                infrastructure.IsComposedOf = null;
            }
        }

        return (_mapper.Map<IEnumerable<Infrastructure>>(result), searchResult);
    }


    public async Task<(IEnumerable<Infrastructure>, SearchAfterResult)> GetInfrastructuresSearchAfter(GetInfrastructuresQueryParameters infrastructuresQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var searchParameters = _mapper.Map<InfrastructureSearchParameters>(infrastructuresQueryParameters);

        var (result, searchAfterResult) = await _searchService.SearchAfter(searchParameters, searchAfterQueryParameters.PageSize, searchAfterQueryParameters.NextPageToken);

        // If query parameters contain ExcludeServices=true, omit IsComposedOf property
        if (infrastructuresQueryParameters.ExcludeServices == true)
        {
            foreach (var infrastructure in result)
            {
                infrastructure.IsComposedOf = null;
            }
        }

        return (_mapper.Map<IEnumerable<Infrastructure>>(result), searchAfterResult);
    }

    public async Task<Infrastructure?> GetInfrastructure(string urn)
    {
        var result = await _searchService.GetSingle(urn);

        return _mapper.Map<Infrastructure>(result);
    }
}