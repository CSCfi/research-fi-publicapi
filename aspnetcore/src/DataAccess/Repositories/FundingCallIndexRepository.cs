using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSC.PublicApi.Service.Models.FundingCall;
using Microsoft.EntityFrameworkCore;

namespace CSC.PublicApi.DataAccess.Repositories;

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