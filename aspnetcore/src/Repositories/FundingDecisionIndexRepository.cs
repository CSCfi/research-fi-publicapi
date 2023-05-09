using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models.FundingDecision;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using FundingCall = CSC.PublicApi.Service.Models.FundingCall.FundingCall;

namespace CSC.PublicApi.Repositories;

public class FundingDecisionIndexRepository : IndexRepositoryBase<FundingDecision>
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    public FundingDecisionIndexRepository(ApiDbContext context, IMapper mapper, IMemoryCache memoryCache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    protected override IQueryable<FundingDecision> GetAll()
    {
        return _context
            .Set<DimFundingDecision>()
            .AsNoTracking()
            .AsSplitQuery()
            .Where(fd => fd.Id != -1)
            .ProjectTo<FundingDecision>(_mapper.ConfigurationProvider);
    }

    public override List<object> PerformInMemoryOperations(List<object> objects)
    {
        objects.ForEach(o =>
        {
            if (o is not FundingDecision fundingDecision)
            {
                return;
            }

            SetAkatemiaConsortia(fundingDecision);

            SetFrameworkProgramme(fundingDecision);

            SetCallProgrammes(fundingDecision);
        });

        return objects;
    }

    private void SetCallProgrammes(FundingDecision fundingDecision)
    {
        if (fundingDecision.CallProgramme is null || fundingDecision.CallProgramme.Id == -1)
        {
            return;
        }

        var callProgramme = fundingDecision.CallProgramme;
        
        // EU funding
        if (callProgramme.SourceDescription == "eu_funding")
        {
            fundingDecision.Topic = new Topic
            {
                NameFi = callProgramme.NameFi,
                NameSv = callProgramme.NameSv,
                NameEn = callProgramme.NameEn,
                TopicId = callProgramme.Abbreviation,
                EuCallId = callProgramme.EuCallId
            };

            if (!_memoryCache.TryGetValue(MemoryCacheKeys.FundingDecisionByAbbreviationAndEuCallId(callProgramme.Abbreviation, callProgramme.EuCallId), out List<string?> foundFundingCalls))
            {
                return;
            }
            
            fundingDecision.CallProgrammes = new List<CallProgramme?>();
            
            foreach (var sourceId in foundFundingCalls)
            {
                if (_memoryCache.TryGetValue(MemoryCacheKeys.FundingDecisionBySourceId(sourceId), out FundingCall fundingCall))
                {
                    fundingDecision.CallProgrammes.Add(new CallProgramme
                    {
                        NameFi = fundingCall.NameFi,
                        NameSv = fundingCall.NameSv,
                        NameEn = fundingCall.NameEn,
                        SourceId = fundingCall.SourceId
                    });
                }
            }
        }
        // Non-EU funding
        else
        {
            fundingDecision.CallProgrammes = new List<CallProgramme?>
            {
                fundingDecision.CallProgramme
            };
        }
    }

    /// <summary>
    /// FrameworkProgramme is the "deepest" (grand)parent of the decision's CallProgramme.
    /// Recursion is impossible with AutoMapper projections so let's use this.
    /// </summary>
    /// <param name="fundingDecision"></param>
    private static void SetFrameworkProgramme(FundingDecision fundingDecision)
    {
        fundingDecision.FrameworkProgramme =
            fundingDecision.CallProgrammeParent6
            ?? fundingDecision.CallProgrammeParent5
            ?? fundingDecision.CallProgrammeParent4
            ?? fundingDecision.CallProgrammeParent3
            ?? fundingDecision.CallProgrammeParent2?.ToFrameworkProgramme()
            ?? fundingDecision.CallProgrammeParent1?.ToFrameworkProgramme();
    }

    /// <summary>
    /// For akatemia decisions we move consortia from temporary property to the main one. 
    /// </summary>
    /// <param name="fundingDecision"></param>
    private static void SetAkatemiaConsortia(FundingDecision fundingDecision)
    {
        if (fundingDecision.OrganizationConsortia?.Any() != true)
        {
            fundingDecision.OrganizationConsortia = fundingDecision.OrganizationConsortia2;
        }
    }
}