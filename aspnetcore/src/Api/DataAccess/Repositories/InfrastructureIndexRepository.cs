using Api.DatabaseContext;
using Api.Models.Infrastructure;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess.Repositories
{
    public class InfrastructureIndexRepository : IIndexRepository<Infrastructure>
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public InfrastructureIndexRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IAsyncEnumerable<Infrastructure> GetAllAsync()
        {
            return _context.DimInfrastructures
                .Where(infrastructure => infrastructure.Id != -1)
                .ProjectTo<Infrastructure>(_mapper.ConfigurationProvider)
                .AsAsyncEnumerable();
        }
    }
}
