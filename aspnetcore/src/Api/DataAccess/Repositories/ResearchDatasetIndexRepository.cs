using Api.DatabaseContext;
using Api.Models.ResearchDataset;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess.Repositories
{
    public class ResearchDatasetIndexRepository : IIndexRepository<ResearchDataset>
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public ResearchDatasetIndexRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IAsyncEnumerable<ResearchDataset> GetAllAsync()
        {
            return _context.DimResearchDatasets
                .Where(dataset => dataset.Id != -1)
                .ProjectTo<ResearchDataset>(_mapper.ConfigurationProvider)
                .AsAsyncEnumerable();
        }
    }
}
