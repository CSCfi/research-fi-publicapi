using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models.FundingDecision;
using Microsoft.EntityFrameworkCore;

namespace CSC.PublicApi.DataAccess.Repositories;

public class FundingDecisionIndexRepository : IndexRepositoryBase<FundingDecision>
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;

    public FundingDecisionIndexRepository(ApiDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public override IQueryable<FundingDecision> GetAll()
    {
        return _context
            .Set<DimFundingDecision>()
            .AsNoTracking()
            .AsSplitQuery()
            .Where(fd => fd.Id != -1)
            .ProjectTo<FundingDecision>(_mapper.ConfigurationProvider);
    }

    public override List<object> PerformInMemoryOperations(List<object> objects)
    {
        objects.ForEach(o =>
        {
            var fd = (FundingDecision)o;
            // For akatemia decisions we move consortia from temporary property to the main one.
            if (fd.OrganizationConsortia?.Any() != true)
            {
                fd.OrganizationConsortia = fd.OrganizationConsortia2;
            }

            // FrameworkProgramme is the "deepest" (grand)parent of the decision's CallProgramme.
            // Recursion is impossible with AutoMapper projections so let's use this.
            fd.FrameworkProgramme =
                fd.CallProgrammeParent6
                ?? fd.CallProgrammeParent5
                ?? fd.CallProgrammeParent4
                ?? fd.CallProgrammeParent3
                ?? fd.CallProgrammeParent2
                ?? fd.CallProgrammeParent1;


        });
        return objects;
    }


}