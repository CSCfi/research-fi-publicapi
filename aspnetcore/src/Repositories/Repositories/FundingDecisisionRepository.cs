using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSC.PublicApi.DataAccess.Repositories;

public class FundingDecisionRepository : GenericRepository<DimFundingDecision>, IFundingDecisionRepository
{
    public FundingDecisionRepository(ApiDbContext context) : base(context)
    {
    }

    public IAsyncEnumerable<DimFundingDecision> GetAllAsync()
    {
        return dbSet
            .AsNoTracking()
            .Include(fd => fd.DimTypeOfFunding)
            .Include(fd => fd.BrParticipatesInFundingGroups)
            // TODO: check if these are needed
            //.Where(fd =>
            //    fd.DimTypeOfFunding.TypeId != "62" &&
            //    fd.DimTypeOfFunding.TypeId != "66" &&
            //    fd.DimTypeOfFunding.TypeId != "69")

            .Where(fd => fd.Id != -1)
            .AsAsyncEnumerable();
    }
}