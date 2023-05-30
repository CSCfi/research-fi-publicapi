using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.Service.Models.FundingCall;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CSC.PublicApi.Repositories;

public class FundingCallIndexRepository : IndexRepositoryBase<FundingCall>
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;

    public FundingCallIndexRepository(ApiDbContext context, IMapper mapper, IMemoryCache memoryCache) : base(memoryCache)
    {
        _context = context;
        _mapper = mapper;
    }

    protected override IQueryable<FundingCall> GetAll()
    {
        return _context.DimCallProgrammes
            .AsNoTracking()
            .AsSplitQuery()
            .Where(callProgramme => callProgramme.Id != -1)
            .ProjectTo<FundingCall>(_mapper.ConfigurationProvider);
    }
    
    public override List<object> PerformInMemoryOperations(List<object> entities)
    {
        entities.ForEach(o =>
        {
            if (o is not FundingCall fundingCall)
            {
                return;
            }

            HandleEmptyCollections(fundingCall);
        });
        return entities;
    }

    private static void HandleEmptyCollections(FundingCall fundingCall)
    {
        if (fundingCall.Foundations != null && !fundingCall.Foundations.Any())
        {
            fundingCall.Foundations = null;
        }
        
        if (fundingCall.Categories != null && !fundingCall.Categories.Any())
        {
            fundingCall.Categories = null;
        }
    }
}