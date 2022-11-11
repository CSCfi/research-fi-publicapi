using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.Service.Models.Infrastructure;

namespace CSC.PublicApi.Repositories;

public class InfrastructureIndexRepository : IndexRepositoryBase<Infrastructure>
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;

    public InfrastructureIndexRepository(ApiDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    protected override IQueryable<Infrastructure> GetAll()
    {
        return _context.DimInfrastructures
            .Where(infrastructure => infrastructure.Id != -1)
            .ProjectTo<Infrastructure>(_mapper.ConfigurationProvider);
    }
}