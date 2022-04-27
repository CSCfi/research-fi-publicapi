using Api.DatabaseContext;
using Api.Models.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess.Repositories
{
    public class FundingDecisionRepository : GenericRepository<DimFundingDecision>, IFundingDecisionRepository
    {
        public FundingDecisionRepository(ApiDbContext context) : base(context)
        {
        }

        public new IAsyncEnumerable<DimFundingDecision> GetAllAsync()
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
}
