using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSC.PublicApi.DatabaseContext;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.ResearchDataset;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Version = CSC.PublicApi.Service.Models.ResearchDataset.Version;

namespace CSC.PublicApi.Repositories;

public class ResearchDatasetIndexRepository : IndexRepositoryBase<ResearchDataset>
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;

    public ResearchDatasetIndexRepository(ApiDbContext context, IMapper mapper, IMemoryCache memoryCache) : base(memoryCache)
    {
        _context = context;
        _mapper = mapper;
    }

    protected override IQueryable<ResearchDataset> GetAll()
    {
        return _context.DimResearchDatasets
            .AsNoTracking()
            .AsSplitQuery()
            .Where(dataset => dataset.Id != -1)
            .Include(d => d.FactContributions)
            .ThenInclude(f => f.DimOrganization)
            .ProjectTo<ResearchDataset>(_mapper.ConfigurationProvider);
    }

    protected override IQueryable<ResearchDataset> GetChunk(int skipAmount, int takeAmount)
    {
        return _context.DimResearchDatasets
            .OrderBy(dataset => dataset.Id)
            .Skip(skipAmount)
            .Take(takeAmount)
            .AsNoTracking()
            .AsSplitQuery()
            .Where(dataset => dataset.Id != -1)
            .Include(d => d.FactContributions)
            .ThenInclude(f => f.DimOrganization)
            .ProjectTo<ResearchDataset>(_mapper.ConfigurationProvider);
    }

    public override List<object> PerformInMemoryOperations(List<object> entities)
    {
        entities.ForEach(o =>
        {
            if (o is not ResearchDataset researchDataset)
            {
                return;
            }

            HandleContributorOrganizations(researchDataset);
            HandleEmptyContributors(researchDataset);
            HandleIsLatestVersion(researchDataset);
            HandleEmptyCollections(researchDataset);
            HandleResearchfiUrl(researchDataset);
        });
        return entities;
    }

    public override object PerformInMemoryOperation(object entity)
    {
        ResearchDataset researchDataset = (ResearchDataset)entity;
        HandleContributorOrganizations(researchDataset);
        HandleEmptyContributors(researchDataset);
        HandleIsLatestVersion(researchDataset);
        HandleEmptyCollections(researchDataset);
        HandleResearchfiUrl(researchDataset);
        return researchDataset;
    }

    private static void HandleContributorOrganizations(ResearchDataset researchDataset)
    {
        if (researchDataset.ContributorsHelper == null)
        {
            return;
        }

        researchDataset.Contributors = new List<Contributor>();
        
        foreach (var contributorHelper in researchDataset.ContributorsHelper)
        {
            var contributor = new Contributor
            {
                // Determine the correct organization based on available IDs
                // Priority:
                // 1. FactContribution_DimOrganizationId
                // 2. FactContribution_DimIdentifierlessData_DimOrganizationId
                // 3. FactContribution_DimIdentifierlessDataId
                Organization = contributorHelper.FactContribution_DimOrganizationId != -1
                    ? contributorHelper.Organization_From_FactContribution_DimOrganization
                    : contributorHelper.FactContribution_DimIdentifierlessDataId != -1
                        ? (contributorHelper.FactContribution_DimIdentifierlessData_DimOrganizationId != -1
                            ? contributorHelper.Organization_From_FactContribution_DimIdentifierlessData_DimOrganization
                            : contributorHelper.Organization_From_FactContribution_DimIdentifierlessData)
                        : null,
                Person = contributorHelper.Person,
                Role = contributorHelper.Role
            };

            researchDataset.Contributors.Add(contributor);
        }

        researchDataset.ContributorsHelper = null;
    }

    private static void HandleEmptyCollections(ResearchDataset researchDataset)
    {
        if (researchDataset.Keywords != null && !researchDataset.Keywords.Any())
        {
            researchDataset.Keywords = null;
        }

        if (researchDataset.SubjectHeadings != null && !researchDataset.SubjectHeadings.Any())
        {
            researchDataset.SubjectHeadings = null;
        }

        if (researchDataset.FieldsOfScience != null && !researchDataset.FieldsOfScience.Any())
        {
            researchDataset.FieldsOfScience = null;
        }

        if (researchDataset.Languages != null && !researchDataset.Languages.Any())
        {
            researchDataset.Languages = null;
        }

        if (researchDataset.PersistentIdentifiers != null && !researchDataset.PersistentIdentifiers.Any())
        {
            researchDataset.PersistentIdentifiers = null;
        }

        if (researchDataset.License is { Code: null, NameEn: null, NameFi: null, NameSv: null })
        {
            researchDataset.License = null;
        }

        if (researchDataset.DatasetRelations != null && !researchDataset.DatasetRelations.Any())
        {
            researchDataset.DatasetRelations = null;
        }

        if (researchDataset.VersionSet != null && !researchDataset.VersionSet.Any())
        {
            researchDataset.VersionSet = null;
        }
    }

    /// <summary>
    /// Removes empty contributors from the <see cref="ResearchDataset"/> before storing to elastic search.
    /// </summary>
    private static void HandleEmptyContributors(ResearchDataset researchDataset)
    {
        if (researchDataset.Contributors == null)
        {
            return;
        }

        // Remove organisations with Id == -1 because they are not actual organisations
        foreach (var contributor in researchDataset.Contributors.Where(contributor => contributor.Organization?.Id == "-1"))
        {
            contributor.Organization = null;
        }

        researchDataset.Contributors.RemoveAll(s => s.Organization == null && s.Person == null);
        if (!researchDataset.Contributors.Any())
        {
            researchDataset.Contributors = null;
        }
    }

    /// <summary>
    /// Determine if dataset is the latest version
    /// </summary>
    private static void HandleIsLatestVersion(ResearchDataset researchDataset)
    {
        researchDataset.IsLatestVersion =
            researchDataset.VersionSet == null ||
            researchDataset.VersionSet.Count == 0 ||
            researchDataset.VersionInfo >=
                researchDataset.VersionSet.Max(m => m.VersionNumber);
    }

    private static void HandleResearchfiUrl(ResearchDataset researchDataset)
    {
        researchDataset.ResearchfiUrl = new ResearchfiUrl(resourceType: "research-dataset", id: researchDataset.Id);
    }
}