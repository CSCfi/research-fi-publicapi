using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.Service.Models.Organization;
using Microsoft.Extensions.Caching.Memory;

namespace CSC.PublicApi.Repositories;

public class OrganizationIndexRepository : IndexRepositoryBase<Organization>
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;

    public OrganizationIndexRepository(ApiDbContext context, IMapper mapper, IMemoryCache memoryCache) : base(memoryCache)
    {
        _context = context;
        _mapper = mapper;
    }

    protected override IQueryable<Organization> GetAll()
    {
        return _context.DimOrganizations
            .Where(organization => organization.Id != -1)
            .ProjectTo<Organization>(_mapper.ConfigurationProvider);
    }
}