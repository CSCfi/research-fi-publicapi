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

    public async Task<(IEnumerable<Organization>, SearchResult)> GetOrganizations(GetOrganizationsQueryParameters queryParameters)
    {
        var searchParameters = _mapper.Map<OrganizationSearchParameters>(queryParameters);

        var (result, searchResult) = await _searchService.Search(searchParameters, queryParameters.PageNumber, queryParameters.PageSize);

        return (_mapper.Map<IEnumerable<Organization>>(result), searchResult);
    }
}