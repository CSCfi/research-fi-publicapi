using Api.DatabaseContext;
using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess.Repositories
{
    public class FundingCallRepository : GenericRepository<DimCallProgramme>, IFundingCallRepository
    {
        public FundingCallRepository(ApiDbContext context) : base(context)
        {
        }

        public new IAsyncEnumerable<DimCallProgramme> GetAllAsync()
        {
            return dbSet
                .Include(x => x.DimOrganizations).AsSplitQuery()
                .Include(x => x.DimReferencedata).AsSplitQuery()
                .Include(x => x.DimWebLinks).AsSplitQuery()
                .Include(x => x.DimDateIdOpenNavigation).AsSplitQuery()
                .Include(x => x.DimDateIdDueNavigation).AsSplitQuery()
                .AsAsyncEnumerable();
        }
    }
}
