using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.FundingCall;
using CSC.PublicApi.Service.Models.Organization;
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
            .Where(callProgramme => callProgramme.Id != -1 && callProgramme.DimOrganizations.Count > 0 && callProgramme.IsOpenCall==true)
            .ProjectTo<FundingCall>(_mapper.ConfigurationProvider);
    }

    protected override IQueryable<FundingCall> GetChunk(int skipAmount, int takeAmount)
    {
        return _context.DimCallProgrammes
            .OrderBy(callProgramme => callProgramme.Id)
            .Skip(skipAmount)
            .Take(takeAmount)
            .AsNoTracking()
            .AsSplitQuery()
            .Where(callProgramme => callProgramme.Id != -1 && callProgramme.DimOrganizations.Count > 0 && callProgramme.IsOpenCall==true)
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
            HandleFoundationBusinessID(fundingCall);
            HandleResearchfiUrl(fundingCall);
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

    private void HandleFoundationBusinessID(FundingCall fundingCall)
    {
        // Use Finnish business id (Y-tunnus) from DimPid
        if (fundingCall.Foundations != null) {
            foreach (Foundation foundation in fundingCall.Foundations.ToList()){
                if (MemoryCache.TryGetValue(MemoryCacheKeys.OrganizationById(foundation.Id),
                        out Organization organization))
                {
                    if (organization.Pids != null)
                    {
                        foreach(PersistentIdentifier pid in organization.Pids.ToList())
                        {
                            if (pid.Type.ToLower() == "businessid") {
                                foundation.BusinessId = pid.Content;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private static void HandleResearchfiUrl(FundingCall fundingCall)
    {
        fundingCall.ResearchfiUrl = new ResearchfiUrl(resourceType: "funding-call", id: fundingCall.Id.ToString());
    }
}