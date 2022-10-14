using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.Service.Models.ResearchDataset;
using Microsoft.EntityFrameworkCore;

namespace CSC.PublicApi.Repositories;

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
            .AsNoTracking()
            .AsSplitQuery()
            .Where(dataset => dataset.Id != -1)
            .ProjectTo<ResearchDataset>(_mapper.ConfigurationProvider);
    }
}