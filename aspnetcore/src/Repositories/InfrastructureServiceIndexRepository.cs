using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.DatabaseContext.Entities;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CSC.PublicApi.Repositories;

public class InfrastructureServiceIndexRepository : IndexRepositoryBase<CSC.PublicApi.Service.Models.Infrastructure.Service>
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;

    public InfrastructureServiceIndexRepository(ApiDbContext context, IMapper mapper, IMemoryCache memoryCache) : base(memoryCache)
    {
        _context = context;
        _mapper = mapper;
    }

    protected override IQueryable<CSC.PublicApi.Service.Models.Infrastructure.Service> GetAll()
    {
        return _context.DimServices
            .AsNoTracking()
            .AsSplitQuery()
            .Where(service => service.Id != -1)
            .ProjectTo<CSC.PublicApi.Service.Models.Infrastructure.Service>(_mapper.ConfigurationProvider);
    }

    protected override IQueryable<CSC.PublicApi.Service.Models.Infrastructure.Service> GetChunk(int skipAmount, int takeAmount)
    {
        return _context.DimServices
            .OrderBy(service => service.Id)
            .Skip(skipAmount)
            .Take(takeAmount)
            .AsNoTracking()
            .AsSplitQuery()
            .Where(service => service.Id != -1)
            .ProjectTo<CSC.PublicApi.Service.Models.Infrastructure.Service>(_mapper.ConfigurationProvider);
    }

    public override List<object> PerformInMemoryOperations(List<object> entities)
    {
        entities.ForEach(o =>
        {
            if (o is not CSC.PublicApi.Service.Models.Infrastructure.Service service)
            {
                return;
            }

            //HandleOrganizations(service);
            HandleServicePids(service);
            HandleIsPartOf(service);
        });
        return entities;
    }

    public override object PerformInMemoryOperation(object entity)
    {
        CSC.PublicApi.Service.Models.Infrastructure.Service service = (CSC.PublicApi.Service.Models.Infrastructure.Service)entity;

        //HandleOrganizations(service);
        HandleServicePids(service);
        HandleIsPartOf(service);
        return service;
    }

    private void HandleServicePids(CSC.PublicApi.Service.Models.Infrastructure.Service service)
    {
        if (service.Pids == null || !service.Pids.Any())
        {
            return;
        }

        service.ServiceIdentifier = new(){
            PersistentIdentifierURN = null,
            PersistentIdentifierURNLink = null,
            OtherPid = new List<PidAttributes>()
        };

        foreach (PersistentIdentifier pid in service.Pids)
        {
            if (pid.Type.ToLower() == "urn")
            {
                service.ServiceIdentifier.PersistentIdentifierURN = pid.Content;
                service.ServiceIdentifier.PersistentIdentifierURNLink = "https://urn.fi/" + pid.Content;
            }
            else
            {
                service.ServiceIdentifier.OtherPid.Add(new PidAttributes
                {
                    Pid = pid.Content,
                    PidType = pid.Type.ToLower()
                });
            }
        }

        // Clear DimPids after processing
        service.Pids = null;
    }

    private void HandleIsPartOf(CSC.PublicApi.Service.Models.Infrastructure.Service service)
    {
        if (service.DimInfrastructureId <= 0)
        {
            return;
        }
        else
        {
            // Initialize IsPartOf
            service.IsPartOf ??= new ServiceInfrastructure();
            service.IsPartOf.InfraIdentifier = new(){
                PersistentIdentifierURN = null,
                PersistentIdentifierURNLink = null,
                OtherPid = new List<PidAttributes>()
            };
        }

        // InfraIdentifier
        service.IsPartOf.Pids = _context.DimPids
            .AsNoTracking()
            .Where(pid => pid.DimInfrastructureId == service.DimInfrastructureId)
            .Select(pid => new PersistentIdentifier
            {
                Type = pid.PidType,
                Content = pid.PidContent
            })
            .ToList();
        foreach (PersistentIdentifier pid in service.IsPartOf.Pids)
        {
            if (pid.Type.ToLower() == "urn")
            {
                service.IsPartOf.InfraIdentifier.PersistentIdentifierURN = pid.Content;
                service.IsPartOf.InfraIdentifier.PersistentIdentifierURNLink = "https://urn.fi/" + pid.Content;
            }
            else
            {
                service.IsPartOf.InfraIdentifier.OtherPid.Add(new PidAttributes
                {
                    Pid = pid.Content,
                    PidType = pid.Type.ToLower()
                });
            }
        }
        service.IsPartOf.Pids = null;

        // OrganizationParticipatesInfrastructure
        List<int> participatingOrganizationIds = _context.FactContributions
            .Where(fc => fc.DimInfrastructureId == service.DimInfrastructureId && fc.ContributionType == "organization_participate_infrastructure")
            .AsNoTracking()
            .Select(fc => fc.DimOrganizationId)
            .Distinct()
            .ToList();
        if (participatingOrganizationIds.Any())
        {
            service.IsPartOf.OrganizationParticipatesInfrastructure = new List<ResearchOrganization>();
            foreach (var orgId in participatingOrganizationIds)
            {
                var participatingOrganization = GetOrganization(orgId);
                if (participatingOrganization != null)
                {
                    var researchOrg = new ResearchOrganization
                    {
                        DimOrganizationId = orgId,
                        OrganizationName = new LanguageVariant
                        {
                            Fi = participatingOrganization?.NameFi,
                            Sv = participatingOrganization?.NameSv,
                            En = participatingOrganization?.NameEn
                        }
                    }; 
                    SetResearchOrganizationIdentifiers(researchOrg, participatingOrganization);
                    researchOrg.DimOrganizationId = null;
                    service.IsPartOf.OrganizationParticipatesInfrastructure.Add(researchOrg);
                }
            }
        }

        // ResponsibleOrganization
        DimInfrastructure relatedInfrastructure = _context.DimInfrastructures
            .AsNoTracking()
            .First(infra => infra.Id == service.DimInfrastructureId);
        if (relatedInfrastructure != null && relatedInfrastructure.ResponsibleOrganizationId > 0)
        {
            var organization = GetOrganization(relatedInfrastructure.ResponsibleOrganizationId);
            if (organization != null)
            {
                service.IsPartOf.ResponsibleOrganization = new ResearchOrganization
                {
                    DimOrganizationId = relatedInfrastructure.ResponsibleOrganizationId,
                    OrganizationName = new LanguageVariant
                    {
                        Fi = organization?.NameFi,
                        Sv = organization?.NameSv,
                        En = organization?.NameEn
                    }
                };

                SetResearchOrganizationIdentifiers(service.IsPartOf.ResponsibleOrganization, organization);
            }
            service.IsPartOf.ResponsibleOrganization.DimOrganizationId = null;
        }

        // InfraName
        List<DimDescriptiveItem> dimDescriptiveItems = _context.DimDescriptiveItems
            .AsNoTracking()
            .Where(di => di.DimInfrastructureId == service.DimInfrastructureId && di.DescriptiveItemType == "name")
            .ToList();
        if (dimDescriptiveItems.Any())
        {
            service.IsPartOf.InfraName = new List<DescriptiveText>();
            foreach (var item in dimDescriptiveItems)
            {
                service.IsPartOf.InfraName.Add(new DescriptiveText
                {
                    TextContent = item.DescriptiveItem,
                    TextLanguage = item.DescriptiveItemLanguage,
                });
            }
            if (!service.IsPartOf.InfraName.Any())
            {
                service.IsPartOf.InfraName = null;
            }
        }

        // Esfri
        service.IsPartOf.Esfri = _context.FactReferencedata
            .AsNoTracking()
            .Where(rd => rd.DimInfrastructureId == service.DimInfrastructureId && rd.DimReferencedata.CodeScheme == "ESFRI-Domain")
            .Select(rd => new CSC.PublicApi.Service.Models.Infrastructure.ReferenceData
            {
                CodeValue = rd.DimReferencedata.CodeValue,
                CodeDescription = new LanguageVariant
                {
                    Fi = rd.DimReferencedata.NameFi,
                    Sv = rd.DimReferencedata.NameSv,
                    En = rd.DimReferencedata.NameEn
                }
            })
            .ToList();
        if (!service.IsPartOf.Esfri.Any())
        {
            service.IsPartOf.Esfri = null;
        }
    }

    private void SetResearchOrganizationIdentifiers(ResearchOrganization targetOrganization, Service.Models.Organization.Organization organization)
    {
        string pidTypeBusinessId = "businessid";
        string pidTypeRorId = "rorid";

        targetOrganization.OrganizationIdentifier = organization.Pids
            .FirstOrDefault(pid => pid.Type.ToLower() == pidTypeBusinessId)?.Content;

        targetOrganization.OrganizationIdentifierROR = organization.Pids
            .FirstOrDefault(pid => pid.Type.ToLower() == pidTypeRorId)?.Content;
    }
}