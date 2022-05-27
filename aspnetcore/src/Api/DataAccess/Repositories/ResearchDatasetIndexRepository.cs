using Api.DatabaseContext;
using Api.Models.ResearchDataset;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Api.DataAccess.Repositories
{
    public class ResearchDatasetIndexRepository : IndexRepositoryBase<ResearchDataset>
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public ResearchDatasetIndexRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override IQueryable<ResearchDataset> GetAll()
        {
            return _context.DimResearchDatasets
                .Where(dataset => dataset.Id != -1)
                .ProjectTo<ResearchDataset>(_mapper.ConfigurationProvider);
        }
    }
}
