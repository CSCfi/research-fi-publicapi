using Api.DatabaseContext;
using Api.Maps;
using Api.Models.Entities;
using Api.Models.FundingCall;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess.Repositories
{
    public class FundingCallIndexRepository : IIndexRepository<Models.FundingCall.FundingCall>
    {
        private readonly ApiDbContext _context;
        private readonly IMapper<DimCallProgramme, Models.FundingCall.FundingCall> _mapper;

        public FundingCallIndexRepository(ApiDbContext context, IMapper<DimCallProgramme, Models.FundingCall.FundingCall> mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IAsyncEnumerable<Models.FundingCall.FundingCall> GetAllAsync()
        {
            return _context.DimCallProgrammes
                .Include(x => x.DimOrganizations).AsSplitQuery()
                .Include(x => x.DimReferencedata).AsSplitQuery()
                //.Include(x => x.DimWebLinks).AsSplitQuery()
                .Include(x => x.DimDateIdOpenNavigation).AsSplitQuery()
                .Include(x => x.DimDateIdDueNavigation).AsSplitQuery()
                .Where(callProgramme => callProgramme.Id != -1)
                .Select(x => _mapper.Map(x))
                .AsAsyncEnumerable();
        }
    }
}
