using Api.DatabaseContext;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using FundingCall = Api.Models.FundingCall.FundingCall;

namespace Api.DataAccess.Repositories
{
    public class FundingCallIndexRepository : IndexRepositoryBase<FundingCall>
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public FundingCallIndexRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override IQueryable<FundingCall> GetAll()
        {
            return _context.DimCallProgrammes
                .AsNoTracking()
                .AsSplitQuery()
                .Where(callProgramme => callProgramme.Id != -1)
                .ProjectTo<FundingCall>(_mapper.ConfigurationProvider);
        }
    }
}
