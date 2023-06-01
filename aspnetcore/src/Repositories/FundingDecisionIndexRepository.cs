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

    public FundingDecisionIndexRepository(ApiDbContext context, IMapper mapper, IMemoryCache memoryCache) : base(memoryCache)
    {
        _context = context;
        _mapper = mapper;
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

            SetFundingReceivers(fundingDecision);
            SetFunder(fundingDecision);
            SetFrameworkProgramme(fundingDecision);
            SetCallProgrammes(fundingDecision);
        });

        return objects;
    }

    private void SetFunder(FundingDecision fundingDecision)
    {
        if (!fundingDecision.FunderId.HasValue)
        {
            return;
        }
        
        fundingDecision.Funder = GetOrganization(fundingDecision.FunderId.Value)?.ToFundingDecisionOrganization();
    }

    private void SetFundingReceivers(FundingDecision fundingDecision)
    {
        fundingDecision.FundingReceivers = new List<FundingReceiver>();
        
        HandleFundingGroupPerson(fundingDecision);
        SetOrganizationConsortia(fundingDecision);

        if (!fundingDecision.FundingReceivers.Any())
        {
            fundingDecision.FundingReceivers = null;
        }
    }

    private FundingReceiver ToFundingReceiver(FundingGroupPerson fundingGroupPerson)
    {
        var receiver = new FundingReceiver
        {
            Person = fundingGroupPerson.Person,
            Organization = GetOrganization(fundingGroupPerson.OrganizationId)?.ToFundingDecisionOrganization(),
            RoleInFundingGroup = fundingGroupPerson.RoleInFundingGroup,
            ShareOfFundingInEur = fundingGroupPerson.ShareOfFundingInEur,
            FunderProjectNumber = fundingGroupPerson.SourceId
        };

        if (receiver.Organization?.Pids != null && !receiver.Organization.Pids.Any())
        {
            receiver.Organization.Pids = null;
        }

        return receiver;
    }

    private void HandleFundingGroupPerson(FundingDecision fundingDecision)
    {
        var fundingGroupPersons = new Dictionary<string, FundingReceiver>();
        
        if (fundingDecision.SelfFundingGroupPerson != null)
        {
            foreach (var fundingGroupPerson in fundingDecision.SelfFundingGroupPerson)
            {
                fundingGroupPersons.TryAdd(fundingGroupPerson.SourceId, ToFundingReceiver(fundingGroupPerson));
            }
        }

        if (fundingDecision.ParentFundingGroupPerson != null)
        {
            foreach (var fundingGroupPerson in fundingDecision.ParentFundingGroupPerson)
            {
                fundingGroupPersons.TryAdd(fundingGroupPerson.SourceId, ToFundingReceiver(fundingGroupPerson));
            }
        }

        if (!fundingGroupPersons.Values.Any())
        {
            return;
        }

        fundingDecision.FundingReceivers.AddRange(fundingGroupPersons.Values.ToList());
    }

    private void SetOrganizationConsortia(FundingDecision fundingDecision)
    {
        if (fundingDecision.OrganizationConsortia == null)
        {
            return;
        }

        foreach (var organizationConsortium in fundingDecision.OrganizationConsortia)
        {
            fundingDecision.FundingReceivers.Add(ToFundingReceiver(organizationConsortium));
        }
    }

    private FundingReceiver ToFundingReceiver(OrganizationConsortium organizationConsortium)
    {
        var receiver =  new FundingReceiver
        {
            Organization = GetOrganization(organizationConsortium.OrganizationId)?.ToFundingDecisionOrganization(),
            RoleInFundingGroup = organizationConsortium.RoleInConsortium,
            ShareOfFundingInEur = organizationConsortium.ShareOfFundingInEur
        };
        
        if (receiver.Organization?.Pids != null && !receiver.Organization.Pids.Any())
        {
            receiver.Organization.Pids = null;
        }

        return receiver;
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

            if (!MemoryCache.TryGetValue(MemoryCacheKeys.FundingCallByAbbreviationAndEuCallId(callProgramme.Abbreviation, callProgramme.EuCallId), out List<string?> foundFundingCalls))
            {
                return;
            }
            
            fundingDecision.CallProgrammes = new List<CallProgramme>();
            
            foreach (var sourceId in foundFundingCalls)
            {
                if (MemoryCache.TryGetValue(MemoryCacheKeys.FundingCallBySourceId(sourceId), out FundingCall fundingCall))
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
            fundingDecision.CallProgrammes = new List<CallProgramme>
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
}