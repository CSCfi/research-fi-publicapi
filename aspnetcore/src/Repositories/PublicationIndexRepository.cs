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

    public PublicationIndexRepository(ApiDbContext context, IMapper mapper, IMemoryCache memoryCache) : base(memoryCache)
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

    protected override IQueryable<Publication> GetChunk(int skipAmount, int takeAmount)
    {
        return _context.DimPublications
            .OrderBy(publication => publication.Id)
            .Skip(skipAmount)
            .Take(takeAmount)
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
            HandleResearchfiUrl(publication);
        });
        return entities;
    }

    public override object PerformInMemoryOperation(object entity)
    {
        Publication publication = (Publication)entity;

        // Add data from OrgPublication DTOs, which hold data collected from publications related to a co-publication (yhteisjulkaisu/osajulkaisu)
        if (publication.OrgPublicationDatabaseContributionDTOs != null && publication.OrgPublicationDatabaseContributionDTOs.Count > 0)
        {
            List<FactContribution> databaseContributions = new();
            foreach (OrgPublicationDatabaseContributionDTO op in publication.OrgPublicationDatabaseContributionDTOs.AsEnumerable().ToList())
            {
                databaseContributions.AddRange(op.DatabaseContributions);
            }
            publication.DatabaseContributions.AddRange(databaseContributions.DistinctBy(x => x.Name));
        }
        if (publication.OrgPublicationKeywordDTOs != null && publication.OrgPublicationKeywordDTOs.Count > 0)
        {
            List<Keyword> keywords = new();
            foreach (OrgPublicationKeywordDTO kw in publication.OrgPublicationKeywordDTOs.AsEnumerable().ToList())
            {
                keywords.AddRange(kw.Keywords);
            }
            publication.Keywords.AddRange(keywords.DistinctBy(x => x.Value));
        }
        if (publication.OrgPublicationArtPublicatonTypeCategoryDTOs != null && publication.OrgPublicationArtPublicatonTypeCategoryDTOs.Count > 0)
        {
            List<ReferenceData> artPublicationTypeCategories = new();
            foreach (OrgPublicationArtPublicatonTypeCategoryDTO ap in publication.OrgPublicationArtPublicatonTypeCategoryDTOs.AsEnumerable().ToList())
            {
                artPublicationTypeCategories.AddRange(ap.ArtPublicationTypeCategories);
            }
            publication.ArtPublicationTypeCategory.AddRange(artPublicationTypeCategories.DistinctBy(x => x.Code));
        }
        if (publication.OrgPublicationFieldsOfScienceDTOs != null && publication.OrgPublicationFieldsOfScienceDTOs.Count > 0)
        {
            List<ReferenceData> fieldsOfScience = new();
            foreach (OrgPublicationFieldsOfScienceDTO fs in publication.OrgPublicationFieldsOfScienceDTOs.AsEnumerable().ToList())
            {
                fieldsOfScience.AddRange(fs.FieldsOfScience);
            }
            publication.FieldsOfScience.AddRange(fieldsOfScience.DistinctBy(x => x.Code));
        }
        if (publication.OrgPublicationFieldsOfArtDTOs != null && publication.OrgPublicationFieldsOfArtDTOs.Count > 0)
        {
            List<ReferenceData> fieldsOfArt = new();
            foreach (OrgPublicationFieldsOfArtDTO fa in publication.OrgPublicationFieldsOfArtDTOs.AsEnumerable().ToList())
            {
                fieldsOfArt.AddRange(fa.FieldsOfArt);
            }
            publication.FieldsOfArt.AddRange(fieldsOfArt.DistinctBy(x => x.Code));
        }
         
        HandleIssnAndIsbn(publication);
        HandleEmptyCollections(publication);
        HandleOrganizations(publication);
        HandleAuthors(publication);
        HandleParentPublications(publication);
        HandlePeerReviewed(publication);
        HandleResearchfiUrl(publication);
        return publication;
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
                var organization = GetOrganization(contribution.OrganizationId.Value);
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
                                list.Add(organization.LocalOrganizationUnitId.ToString());
                            }
                            else
                            {
                                organizationUnits.Add(organization.ParentId.Value, new List<string> { organization.LocalOrganizationUnitId });    
                            }

                            if (!mainOrganizations.TryGetValue(organization.ParentId.Value, out _))
                            {
                                var mainOrganization = GetOrganization(organization.ParentId.Value);
                                if (mainOrganization is not null)
                                {
                                    mainOrganizations.Add(mainOrganization.Id, mainOrganization.OrganizationId);                                    
                                }
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
            
            var organization = GetOrganization(contribution.OrganizationId.Value);
            if (organization is null)
            {
                continue;
            }
            
            mainOrganizations.Add(contribution.OrganizationId.Value, organization.ToMainOrganization());
        }
        
        foreach (var contribution in publication.DatabaseContributions.Where(contribution => contribution.ContributionType == PublicationOrganizationUnit))
        {
            var organization = GetOrganization(contribution.OrganizationId.Value);
            if (organization is null)
            {
                continue;
            }

            if (!mainOrganizations.TryGetValue(organization.ParentId.Value, out var mainOrganization))
            {
                var missingOrganization = GetOrganization(organization.ParentId.Value);
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

    private static void HandleResearchfiUrl(Publication publication)
    {
        // Organization part of a copublication (osajulkaisu) does not have a ResearchfiUrl
        if (!publication.IsOrgPublication)
        {
            publication.ResearchfiUrl = new ResearchfiUrl(resourceType: "publication", id: publication.Id);
        }
    }
}