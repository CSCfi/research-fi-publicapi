using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSC.PublicApi.Repositories;

public class FundingDecisionRepository : GenericRepository<DimFundingDecision>, IFundingDecisionRepository
{
    public FundingDecisionRepository(ApiDbContext context) : base(context)
    {
    }

    public IAsyncEnumerable<DimFundingDecision> GetAllAsync()
    {
        return DbSet
            .AsNoTracking()
            //.Include(fd => fd.DimTypeOfFunding)
            //.Include(fd => fd.BrParticipatesInFundingGroups)
            //.Include(fd => fd.DimFundingDecisionIdParentDecisionNavigation)
            .Where(fd => fd.Id != -1)
            .AsAsyncEnumerable();
    }
}