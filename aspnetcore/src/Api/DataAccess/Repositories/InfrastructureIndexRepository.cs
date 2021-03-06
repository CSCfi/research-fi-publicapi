using Api.DatabaseContext;
using Api.Models.Infrastructure;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Api.DataAccess.Repositories
{
    public class InfrastructureIndexRepository : IndexRepositoryBase<Infrastructure>
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public InfrastructureIndexRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override IQueryable<Infrastructure> GetAll()
        {
            return _context.DimInfrastructures
                .Where(infrastructure => infrastructure.Id != -1)
                .ProjectTo<Infrastructure>(_mapper.ConfigurationProvider);
        }
    }
}
