using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.DatabaseContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSC.PublicApi.Repositories;

public class FundingCallRepository : GenericRepository<DimCallProgramme>, IFundingCallRepository
{
    public FundingCallRepository(ApiDbContext context) : base(context)
    {
    }

    public IAsyncEnumerable<DimCallProgramme> GetAllAsync()
    {
        return DbSet
            .Include(x => x.DimOrganizations).AsSplitQuery()
            .Include(x => x.DimReferencedata).AsSplitQuery()
            //.Include(x => x.DimWebLinks).AsSplitQuery()
            .Include(x => x.DimDateIdOpenNavigation).AsSplitQuery()
            .Include(x => x.DimDateIdDueNavigation).AsSplitQuery()
            .Where(callProgramme => callProgramme.Id != -1)
            .AsAsyncEnumerable();
    }
}