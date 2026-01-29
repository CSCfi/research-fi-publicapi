using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CSC.PublicApi.Repositories;

public class InfrastructureIndexRepository : IndexRepositoryBase<Infrastructure>
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;

    public InfrastructureIndexRepository(ApiDbContext context, IMapper mapper, IMemoryCache memoryCache) : base(memoryCache)
    {
        _context = context;
        _mapper = mapper;
    }

    protected override IQueryable<Infrastructure> GetAll()
    {
        return _context.DimInfrastructures
            .AsNoTracking()
            .AsSplitQuery()
            .Where(infrastructure => infrastructure.Id != -1)
            .ProjectTo<Infrastructure>(_mapper.ConfigurationProvider);
    }

    protected override IQueryable<Infrastructure> GetChunk(int skipAmount, int takeAmount)
    {
        return _context.DimInfrastructures
            .AsNoTracking()
            .AsSplitQuery()
            .OrderBy(infrastructure => infrastructure.Id)
            .Skip(skipAmount)
            .Take(takeAmount)
            .Where(infrastructure => infrastructure.Id != -1)
            .ProjectTo<Infrastructure>(_mapper.ConfigurationProvider);
    }

    public override List<object> PerformInMemoryOperations(List<object> entities)
    {
        entities.ForEach(o =>
        {
            if (o is not Infrastructure infrastructure)
            {
                return;
            }

            HandleOrganizations(infrastructure);
            HandleInfraPids(infrastructure);
            HandleIsComposedOfPids(infrastructure);
            HandleRelationToInfraPids(infrastructure);
            HandleResearchfiUrl(infrastructure);
            HandleEmptyCollections(infrastructure);
        });
        return entities;
    }

    public override object PerformInMemoryOperation(object entity)
    {
        Infrastructure infrastructure = (Infrastructure)entity;
        HandleOrganizations(infrastructure);
        HandleInfraPids(infrastructure);
        HandleIsComposedOfPids(infrastructure);
        HandleRelationToInfraPids(infrastructure);
        HandleResearchfiUrl(infrastructure);
        HandleEmptyCollections(infrastructure);
        return infrastructure;
    }

    private void HandleOrganizations(Infrastructure infrastructure)
    {
        if (infrastructure.ResponsibleOrganization?.DimOrganizationId != null)
        {
            var organization = GetOrganization(infrastructure.ResponsibleOrganization.DimOrganizationId.Value);
            if (organization != null)
            {
                SetResearchOrganizationIdentifiers(infrastructure.ResponsibleOrganization, organization);
            }
            infrastructure.ResponsibleOrganization.DimOrganizationId = null;
        }

        foreach (var researchOrg in infrastructure.OrganizationParticipatesInfrastructure ?? [])
        {
            if (researchOrg.DimOrganizationId != null)
            {
                var organization = GetOrganization(researchOrg.DimOrganizationId.Value);
                if (organization != null)
                {
                    SetResearchOrganizationIdentifiers(researchOrg, organization);
                }
                researchOrg.DimOrganizationId = null;
            }
        }
    }

    private void HandleInfraPids(Infrastructure infrastructure)
    {
        if (infrastructure.Pids == null || !infrastructure.Pids.Any())
        {
            infrastructure.Pids = null;
            return;
        }

        infrastructure.InfraIdentifier = new(){
            PersistentIdentifierURN = null,
            PersistentIdentifierURNLink = null,
            OtherPid = new List<PidAttributes>(),
            LocalIdentifier = infrastructure.InfraIdentifier?.LocalIdentifier ?? infrastructure.LocalIdentifier
        };

        foreach (PersistentIdentifier pid in infrastructure.Pids)
        {
            if (pid.Type.ToLower() == "urn")
            {
                infrastructure.InfraIdentifier.PersistentIdentifierURN = pid.Content;
                infrastructure.InfraIdentifier.PersistentIdentifierURNLink = "https://urn.fi/" + pid.Content;
            }
            else
            {
                infrastructure.InfraIdentifier.OtherPid.Add(new PidAttributes
                {
                    Pid = pid.Content,
                    PidType = pid.Type.ToLower()
                });
            }
        }

        // Clear Pids after processing
        infrastructure.Pids = null;
    }

    private void HandleIsComposedOfPids(Infrastructure infrastructure)
    {
        if (infrastructure.IsComposedOf == null || !infrastructure.IsComposedOf.Any())
        {
            infrastructure.IsComposedOf = null;
            return;
        }
        foreach (InfrastructureService service in infrastructure.IsComposedOf)
        {
            if (service.Pids == null || !service.Pids.Any())
            {
                service.Pids = null;
                continue;
            }

            service.ServiceIdentifier = new(){
                PersistentIdentifierURN = null,
                PersistentIdentifierURNLink = null,
                OtherPid = new List<PidAttributes>(),
                LocalIdentifier = service.LocalIdentifier
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

            // Clear Pids after processing
            service.Pids = null;
        }
    }

    private void HandleRelationToInfraPids(Infrastructure infrastructure)
    {
        if (infrastructure == null || !infrastructure.InfraNetwork.Any())
        {
            return;
        }

        foreach (InfrastructureNetwork infraNetwork in infrastructure.InfraNetwork)
        {
            if (infraNetwork.Pids == null || !infraNetwork.Pids.Any())
            {
                infraNetwork.Pids = null;
                continue;
            }

            infraNetwork.RelationToInfra = new(){
                PersistentIdentifierURN = null,
                PersistentIdentifierURNLink = null,
                OtherPid = new List<PidAttributes>()
            };

            foreach (PersistentIdentifier pid in infraNetwork.Pids)
            {
                if (pid.Type.ToLower() == "urn")
                {
                    infraNetwork.RelationToInfra.PersistentIdentifierURN = pid.Content;
                    infraNetwork.RelationToInfra.PersistentIdentifierURNLink = "https://urn.fi/" + pid.Content;
                }
                else
                {
                    infraNetwork.RelationToInfra.OtherPid.Add(new PidAttributes
                    {
                        Pid = pid.Content,
                        PidType = pid.Type.ToLower()
                    });
                }
            }

            // Clear Pids after processing
            infraNetwork.Pids = null;
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

    private static void HandleResearchfiUrl(Infrastructure infrastructure)
    {
        // Infrastucture's researchfi URL
        if (infrastructure.InfraIdentifier == null || string.IsNullOrEmpty(infrastructure.InfraIdentifier.PersistentIdentifierURN))
        {
            infrastructure.InfraResearchfiURL = null;
            return;
        }
        ResearchfiUrl researchfiUrl = new ResearchfiUrl(
            resourceType: "infrastructure",
            id: infrastructure.InfraIdentifier.PersistentIdentifierURN
        );
        infrastructure.InfraResearchfiURL = new LanguageVariant
        {
            Fi = researchfiUrl.Fi,
            Sv = researchfiUrl.Sv,
            En = researchfiUrl.En
        };

        // Services' researchfi URLs
        foreach (InfrastructureService service in infrastructure.IsComposedOf ?? [])
        {
            if (service.ServiceIdentifier == null || string.IsNullOrEmpty(service.ServiceIdentifier.PersistentIdentifierURN))
            {
                service.ServiceResearchfiURL = null;
                continue;
            }
            ResearchfiUrl serviceResearchfiUrl = new ResearchfiUrl(
                resourceType: "infrastructure-service",
                id: infrastructure.InfraIdentifier.PersistentIdentifierURN,
                infrastructureServiceId: service.ServiceIdentifier.PersistentIdentifierURN
            );
            service.ServiceResearchfiURL = new LanguageVariant
            {
                Fi = serviceResearchfiUrl.Fi,
                Sv = serviceResearchfiUrl.Sv,
                En = serviceResearchfiUrl.En
            };
        }
    }

    private static void HandleEmptyCollections(Infrastructure infrastructure)
    {
        if (infrastructure.InfraName != null && !infrastructure.InfraName.Any()) { infrastructure.InfraName = null; }
        if (infrastructure.InfraDescription != null && !infrastructure.InfraDescription.Any()) { infrastructure.InfraDescription = null; }
        if (infrastructure.FieldOfScience != null && !infrastructure.FieldOfScience.Any()) { infrastructure.FieldOfScience = null; }
        if (infrastructure.InfraNetwork != null && !infrastructure.InfraNetwork.Any()) { infrastructure.InfraNetwork = null; }
        if (infrastructure.OrganizationParticipatesInfrastructure != null && !infrastructure.OrganizationParticipatesInfrastructure.Any()) { infrastructure.OrganizationParticipatesInfrastructure = null; }
        if (infrastructure.InfraHomepage != null && !infrastructure.InfraHomepage.Any()) { infrastructure.InfraHomepage = null; }
        if (infrastructure.InfraContactInformation != null && !infrastructure.InfraContactInformation.Any()) { infrastructure.InfraContactInformation = null; }
        if (infrastructure.Esfri != null && !infrastructure.Esfri.Any()) { infrastructure.Esfri = null; }

        // IsComposedOf
        foreach (InfrastructureService service in infrastructure.IsComposedOf ?? [])
        {
            if (service.ServiceName != null && !service.ServiceName.Any()) { service.ServiceName = null; }
            if (service.ServiceHomepage != null && !service.ServiceHomepage.Any()) { service.ServiceHomepage = null; }
            if (service.ServiceContactInformation != null && !service.ServiceContactInformation.Any()) { service.ServiceContactInformation = null; }
            if (service.ServiceDescription != null && !service.ServiceDescription.Any()) { service.ServiceDescription = null; }
            if (service.ServiceBookingLink != null && !service.ServiceBookingLink.Any()) { service.ServiceBookingLink = null; }
            if (service.ServiceUserRole != null && !service.ServiceUserRole.Any()) { service.ServiceUserRole = null; }
            if (service.ServiceEndUserGuide != null && !service.ServiceEndUserGuide.Any()) { service.ServiceEndUserGuide = null; }
            if (service.ServiceTargetSegment != null && !service.ServiceTargetSegment.Any()) { service.ServiceTargetSegment = null; }
            if (service.ServicePrivacyPolicy != null && !service.ServicePrivacyPolicy.Any()) { service.ServicePrivacyPolicy = null; }
            if (service.ServiceTermsOfUse != null && !service.ServiceTermsOfUse.Any()) { service.ServiceTermsOfUse = null; }
        }
        if (infrastructure.IsComposedOf != null && !infrastructure.IsComposedOf.Any())
        {
             infrastructure.IsComposedOf = null;
        }
    }
}