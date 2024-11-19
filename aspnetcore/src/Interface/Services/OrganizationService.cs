using AutoMapper;
using CSC.PublicApi.ElasticService;
using CSC.PublicApi.ElasticService.SearchParameters;
using ResearchFi.Query;
using Organization = ResearchFi.Organization.Organization;

namespace CSC.PublicApi.Interface.Services;

public class OrganizationService : IOrganizationService
{
    private readonly ILogger<OrganizationService> _logger;
    private readonly IMapper _mapper;
    private readonly ISearchService<OrganizationSearchParameters, Service.Models.Organization.Organization> _searchService;

    public OrganizationService(ILogger<OrganizationService> logger, IMapper mapper, ISearchService<OrganizationSearchParameters, Service.Models.Organization.Organization> searchService)
    {
        _logger = logger;
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task<(IEnumerable<Organization>, SearchResult)> GetOrganizations(GetOrganizationsQueryParameters organizationsQueryParameters, PaginationQueryParameters paginationQueryParameters)
    {
        var searchParameters = _mapper.Map<OrganizationSearchParameters>(organizationsQueryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, paginationQueryParameters.PageNumber, paginationQueryParameters.PageSize);

        return (_mapper.Map<IEnumerable<Organization>>(result), searchResult);
    }


    public async Task<(IEnumerable<Organization>, SearchAfterResult)> GetOrganizationsSearchAfter(GetOrganizationsQueryParameters organizationsQueryParameters, SearchAfterQueryParameters searchAfterQueryParameters)
    {
        var searchParameters = _mapper.Map<OrganizationSearchParameters>(organizationsQueryParameters);

        var (result, searchAfterResult) = await _searchService.SearchAfter(searchParameters, searchAfterQueryParameters.PageSize, searchAfterQueryParameters.NextPageToken);

        return (_mapper.Map<IEnumerable<Organization>>(result), searchAfterResult);
    }
}