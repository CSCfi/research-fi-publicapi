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
            .Where(infrastructure => infrastructure.Id != -1 && infrastructure.SourceDescription != "infra_old")
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
            .Where(infrastructure => infrastructure.Id != -1 && infrastructure.SourceDescription != "infra_old")
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

            // HandleOrganizations(infrastructure);
            // HandleInfraPids(infrastructure);
            // HandleInfraServicesPids(infrastructure);
            // HandleRelationToNationalInfraPids(infrastructure);
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
        HandleInfraServicesPids(infrastructure);
        HandleRelationToNationalInfraPids(infrastructure);
        HandleResearchfiUrl(infrastructure);
        HandleEmptyCollections(infrastructure);
        return infrastructure;
    }

    private void HandleOrganizations(Infrastructure infrastructure)
    {
        if (infrastructure.InfraResponsibleOrganization?.DimOrganizationId != null)
        {
            var organization = GetOrganization(infrastructure.InfraResponsibleOrganization.DimOrganizationId.Value);
            if (organization != null)
            {
                SetResearchOrganizationIdentifiers(infrastructure.InfraResponsibleOrganization, organization);
            }
            infrastructure.InfraResponsibleOrganization.DimOrganizationId = null;
        }

        foreach (var researchOrg in infrastructure.InfraParticipatingOrganizations ?? [])
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
            KeyIdentifierURN = null,
            KeyIdentifierURNLink = null,
            OtherPid = new List<PidAttributes>(),
            LocalIdentifier = infrastructure.InfraIdentifier?.LocalIdentifier ?? infrastructure.LocalIdentifier
        };

        foreach (PersistentIdentifier pid in infrastructure.Pids)
        {
            if (pid.Type.ToLower() == "urn")
            {
                infrastructure.InfraIdentifier.KeyIdentifierURN = pid.Content;
                infrastructure.InfraIdentifier.KeyIdentifierURNLink = RepositoryHelpers.GetURNLink(pid.Content);
            }
            else
            {
                infrastructure.InfraIdentifier.OtherPid.Add(new PidAttributes
                {
                    PidContent = pid.Content,
                    PidType = pid.Type.ToLower()
                });
            }
        }

        // Clear Pids after processing
        infrastructure.Pids = null;
    }

    private void HandleInfraServicesPids(Infrastructure infrastructure)
    {
        if (infrastructure.InfraServices == null || !infrastructure.InfraServices.Any())
        {
            infrastructure.InfraServices = null;
            return;
        }
        foreach (InfrastructureService service in infrastructure.InfraServices)
        {
            if (service.Pids == null || !service.Pids.Any())
            {
                service.Pids = null;
                continue;
            }

            service.ServiceIdentifier = new(){
                KeyIdentifierURN = null,
                KeyIdentifierURNLink = null,
                OtherPid = new List<PidAttributes>(),
                LocalIdentifier = service.LocalIdentifier
            };

            foreach (PersistentIdentifier pid in service.Pids)
            {
                if (pid.Type.ToLower() == "urn")
                {
                    service.ServiceIdentifier.KeyIdentifierURN = pid.Content;
                    service.ServiceIdentifier.KeyIdentifierURNLink = RepositoryHelpers.GetURNLink(pid.Content);
                }
                else
                {
                    service.ServiceIdentifier.OtherPid.Add(new PidAttributes
                    {
                        PidContent = pid.Content,
                        PidType = pid.Type.ToLower()
                    });
                }
            }

            // Clear Pids after processing
            service.Pids = null;
        }
    }

    private void HandleRelationToNationalInfraPids(Infrastructure infrastructure)
    {
        if (infrastructure == null || !infrastructure.InfraRelations.Any())
        {
            return;
        }

        foreach (InfrastructureNetwork infraNetwork in infrastructure.InfraRelations)
        {
            if (infraNetwork.Pids == null || !infraNetwork.Pids.Any())
            {
                infraNetwork.Pids = null;
                continue;
            }

            infraNetwork.RelationToNationalInfra = new(){
                KeyIdentifierURN = null,
                KeyIdentifierURNLink = null,
                OtherPid = new List<PidAttributes>()
            };

            foreach (PersistentIdentifier pid in infraNetwork.Pids)
            {
                if (pid.Type.ToLower() == "urn")
                {
                    infraNetwork.RelationToNationalInfra.KeyIdentifierURN = pid.Content;
                    infraNetwork.RelationToNationalInfra.KeyIdentifierURNLink = RepositoryHelpers.GetURNLink(pid.Content);
                }
                else
                {
                    infraNetwork.RelationToNationalInfra.OtherPid.Add(new PidAttributes
                    {
                        PidContent = pid.Content,
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
        targetOrganization.OrganizationIdentifier = organization.Pids.Select(pid => new PidAttributes
        {
            PidContent = pid.Content,
            PidType = pid.Type.ToLower()
        }).ToList();
    }

    private static void HandleResearchfiUrl(Infrastructure infrastructure)
    {
        // Infrastucture's researchfi URL
        if (infrastructure.InfraIdentifier == null || string.IsNullOrEmpty(infrastructure.InfraIdentifier.KeyIdentifierURN))
        {
            infrastructure.InfraResearchfiURL = null;
            return;
        }
        ResearchfiUrl researchfiUrl = new ResearchfiUrl(
            resourceType: "infrastructure",
            id: infrastructure.InfraIdentifier.KeyIdentifierURN
        );
        infrastructure.InfraResearchfiURL = new LanguageVariant
        {
            Fi = researchfiUrl.Fi,
            Sv = researchfiUrl.Sv,
            En = researchfiUrl.En
        };

        // Services' researchfi URLs
        foreach (InfrastructureService service in infrastructure.InfraServices ?? [])
        {
            if (service.ServiceIdentifier == null || string.IsNullOrEmpty(service.ServiceIdentifier.KeyIdentifierURN))
            {
                service.ServiceResearchfiURL = null;
                continue;
            }
            ResearchfiUrl serviceResearchfiUrl = new ResearchfiUrl(
                resourceType: "infrastructure-service",
                id: infrastructure.InfraIdentifier.KeyIdentifierURN,
                infrastructureServiceId: service.ServiceIdentifier.KeyIdentifierURN
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
        if (infrastructure.InfraRelations != null && !infrastructure.InfraRelations.Any()) { infrastructure.InfraRelations = null; }
        if (infrastructure.InfraParticipatingOrganizations != null && !infrastructure.InfraParticipatingOrganizations.Any()) { infrastructure.InfraParticipatingOrganizations = null; }
        if (infrastructure.InfraHomepage != null && !infrastructure.InfraHomepage.Any()) { infrastructure.InfraHomepage = null; }
        if (infrastructure.InfraContactInformation != null && !infrastructure.InfraContactInformation.Any()) { infrastructure.InfraContactInformation = null; }
        if (infrastructure.ESFRICodes != null && !infrastructure.ESFRICodes.Any()) { infrastructure.ESFRICodes = null; }

        // InfraServices
        foreach (InfrastructureService service in infrastructure.InfraServices ?? [])
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
        if (infrastructure.InfraServices != null && !infrastructure.InfraServices.Any())
        {
             infrastructure.InfraServices = null;
        }
    }
}