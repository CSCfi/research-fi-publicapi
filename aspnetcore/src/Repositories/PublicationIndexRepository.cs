using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Publication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Publication = CSC.PublicApi.Service.Models.Publication.Publication;

namespace CSC.PublicApi.Repositories;

public class PublicationIndexRepository : IndexRepositoryBase<Publication>
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    
    private const string PublicationAuthorOrganization = "publication_author_organization";
    private const string PublicationAuthorOrganizationUnit = "publication_author_organization_unit";
    private const string PublicationOrganization = "publication_organization";
    private const string PublicationOrganizationUnit = "publication_organization_unit";
    private const string PeerReviewedNameFi = "Vertaisarvioitu";
    private const string NotPeerReviewedFi = "Ei-vertaisarvioitu";
    private const string PeerReviewedSv = "Kollegialt utvärderad";
    private const string NotPeerReviewedSv = "Inte kollegialt utvärderad";
    private const string PeerReviewedEn = "Peer-Reviewed";
    private const string NotPeerReviewedEn = "Non Peer-Reviewed";
    private const string PeerReviewedCode = "1";
    private const string NotPeerReviewedCode = "0";

    public PublicationIndexRepository(ApiDbContext context, IMapper mapper, IMemoryCache memoryCache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    protected override IQueryable<Publication> GetAll()
    {
        return _context.DimPublications
            .AsNoTracking()
            .AsSplitQuery()
            .Where(publication => publication.Id != -1)
            .ProjectTo<Publication>(_mapper.ConfigurationProvider);
    }
    
    public override List<object> PerformInMemoryOperations(List<object> entities)
    {
        entities.ForEach(o =>
        {
            if (o is not Publication publication)
            {
                return;
            }
            
            HandleIssnAndIsbn(publication);
            HandleEmptyCollections(publication);
            HandleOrganizations(publication);
            HandleAuthors(publication);
            HandleParentPublications(publication);
            HandlePeerReviewed(publication);

        });
        return entities;
    }

    private static void HandlePeerReviewed(Publication publication)
    {
        if (publication.DatabasePeerReviewed is not null)
        {
            publication.PeerReviewed = new ReferenceData
            {
                Code = publication.DatabasePeerReviewed.Value ? PeerReviewedCode : NotPeerReviewedCode,
                NameFi = publication.DatabasePeerReviewed.Value ? PeerReviewedNameFi : NotPeerReviewedFi,
                NameSv = publication.DatabasePeerReviewed.Value ? PeerReviewedSv : NotPeerReviewedSv,
                NameEn = publication.DatabasePeerReviewed.Value ? PeerReviewedEn : NotPeerReviewedEn,
            };
        }
    }

    private static void HandleParentPublications(Publication publication)
    {
        publication.ParentPublication = new ParentPublication
        {
            Name = publication.ParentPublicationName,
            Type = publication.ParentPublicationType,
            Publisher = publication.ParentPublicationPublisher
        };
    }

    private void HandleAuthors(Publication publication)
    {
        if (publication.DatabaseContributions == null)
        {
            publication.Authors = null;
            return;
        }

        publication.Authors = new List<Author>();
        var persons = new List<Author>();

        var groupByNameId = 
            from contribution in publication.DatabaseContributions.Where(p => p.ContributionType is PublicationAuthorOrganization or PublicationAuthorOrganizationUnit)
            group contribution by contribution.Name.NameId
            into newGroup
            orderby newGroup.Key
            select newGroup;

        foreach (var nameIdGroup in groupByNameId)
        {
            if (nameIdGroup.FirstOrDefault() == null)
            {
                continue;
            }
            
            var person = new Author
            {
                FirstNames = nameIdGroup.FirstOrDefault()?.Name?.FirstNames,
                LastName = nameIdGroup.FirstOrDefault()?.Name?.LastName,
                Orcid = nameIdGroup.FirstOrDefault()?.Name?.Orcid,
                ArtPublicationRole = string.IsNullOrWhiteSpace(nameIdGroup.FirstOrDefault()?.ArtPublicationRole?.Code) ? null : nameIdGroup.FirstOrDefault()?.ArtPublicationRole,
                Organizations = new List<AuthorOrganization>()
            };
            
            var mainOrganizations = new Dictionary<int, string>();
            var organizationUnits = new Dictionary<int, List<string>>();
            
            foreach (var contribution in nameIdGroup)
            {
                var organization = _memoryCache.Get<CSC.PublicApi.Service.Models.Organization.Organization>(contribution.OrganizationId);
                if (organization is null)
                {
                    continue;
                }
                
                switch (contribution.ContributionType)
                {
                    case PublicationAuthorOrganization:
                        mainOrganizations.TryAdd(organization.Id, organization.OrganizationId);
                        break;
                    case PublicationAuthorOrganizationUnit:
                        if (organization.ParentId.HasValue)
                        {
                            if (organizationUnits.TryGetValue(organization.ParentId.Value, out var list))
                            {
                                list.Add(organization.Id.ToString());
                            }
                            else
                            {
                                organizationUnits.Add(organization.ParentId.Value, new List<string> { organization.LocalOrganizationUnitId });    
                            }

                            if (!mainOrganizations.TryGetValue(organization.ParentId.Value, out _))
                            {
                                var mainOrganization = _memoryCache.Get<CSC.PublicApi.Service.Models.Organization.Organization>(organization.ParentId.Value);
                                mainOrganizations.Add(mainOrganization.Id, mainOrganization.OrganizationId);
                            }
                        }

                        break;
                }
            }

            foreach (var mainOrganizationsValue in mainOrganizations)
            {
                var org = new AuthorOrganization
                {
                    Id = mainOrganizationsValue.Value
                };

                if (organizationUnits.TryGetValue(mainOrganizationsValue.Key, out var unit))
                {
                    org.UnitIds = unit;
                }
                
                person.Organizations.Add(org);
            }

            persons.Add(person);
        }

        publication.Authors.AddRange(persons); }

    private void HandleOrganizations(Publication publication)
    {
        if (publication.DatabaseContributions == null)
        {
            publication.Organizations = null;
            return;
        }
            
        var mainOrganizations = new Dictionary<int, Organization>();
        var organizationUnits = new Dictionary<int, OrganizationUnit>();
        
        foreach (var contribution in publication.DatabaseContributions.Where(contribution => contribution.ContributionType == PublicationOrganization))
        {
            if (!contribution.OrganizationId.HasValue || mainOrganizations.ContainsKey(contribution.OrganizationId.Value))
            {
                continue;
            }
            
            var organization = _memoryCache.Get<CSC.PublicApi.Service.Models.Organization.Organization>(contribution.OrganizationId);
            if (organization is null)
            {
                continue;
            }
            
            mainOrganizations.Add(contribution.OrganizationId.Value, organization.ToMainOrganization());
        }
        
        foreach (var contribution in publication.DatabaseContributions.Where(contribution => contribution.ContributionType == PublicationOrganizationUnit))
        {
            var organization = _memoryCache.Get<CSC.PublicApi.Service.Models.Organization.Organization>(contribution.OrganizationId);
            if (organization is null)
            {
                continue;
            }

            if (!mainOrganizations.TryGetValue(organization.ParentId.Value, out var mainOrganization))
            {
                var missingOrganization = _memoryCache.Get<CSC.PublicApi.Service.Models.Organization.Organization>(organization.ParentId.Value);
                if (missingOrganization is null)
                {
                    continue;
                }

                mainOrganization = missingOrganization.ToMainOrganization();
                mainOrganizations.Add(missingOrganization.Id, mainOrganization);
            }

            mainOrganization.Units ??= new List<OrganizationUnit>();

            var organizationUnit = organization.ToOrganizationUnit();
            mainOrganization.Units.Add(organizationUnit);
            organizationUnits.TryAdd(organizationUnit.DatabaseId.Value, organizationUnit);
        }

        publication.Organizations = new List<Organization>();
        foreach (var mainOrganization in mainOrganizations.Values)
        {
            publication.Organizations.Add(mainOrganization);
        }
    }

    private static void HandleEmptyCollections(Publication publication)
    {
        if (publication.Keywords != null && !publication.Keywords.Any())
        {
            publication.Keywords = null;
        }
    }

    private static void HandleIssnAndIsbn(Publication publication)
    {
            var issn = new List<string?>
            {
                publication.Issn1?.Trim(),
                publication.Issn2?.Trim()
            };

            publication.Issn = issn.ToList();
            
            var isbn = new List<string?>
            {
                publication.Isbn1?.Trim(),
                publication.Isbn2?.Trim()
            };

            publication.Issn = issn.Where(n => !string.IsNullOrWhiteSpace(n)).ToList();
            publication.Isbn = isbn.Where(n => !string.IsNullOrWhiteSpace(n)).ToList();
    }
}