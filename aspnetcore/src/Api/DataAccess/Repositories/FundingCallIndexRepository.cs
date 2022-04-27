using Api.DatabaseContext;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using FundingCall = Api.Models.FundingCall.FundingCall;

namespace Api.DataAccess.Repositories
{
    public class FundingCallIndexRepository : IIndexRepository<FundingCall>
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public FundingCallIndexRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IAsyncEnumerable<FundingCall> GetAllAsync()
        {
            return _context.DimCallProgrammes
                .AsNoTracking()
                .Include(x => x.DimOrganizations).AsSplitQuery()
                .Include(x => x.DimReferencedata).AsSplitQuery()
                .Include(x => x.DimDateIdOpenNavigation).AsSplitQuery()
                .Include(x => x.DimDateIdDueNavigation).AsSplitQuery()
                .Where(callProgramme => callProgramme.Id != -1)
                .ProjectTo<FundingCall>(_mapper.ConfigurationProvider)
                .AsAsyncEnumerable();
        }
    }
}
