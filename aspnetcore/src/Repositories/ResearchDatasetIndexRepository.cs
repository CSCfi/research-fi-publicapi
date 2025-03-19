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
            
            HandleEmptyContributors(researchDataset);
            HandleDatasetRelations(researchDataset);
            HandleEmptyCollections(researchDataset);
            HandleResearchfiUrl(researchDataset);
        });
        return entities;
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
    /// Checks all incoming and outgoing versions and creates a single list to the <see cref="ResearchDataset.VersionSet"/> property.
    /// </summary>
    private static void HandleDatasetRelations(ResearchDataset researchDataset)
    {
        var versions = new List<Version>();

        if (researchDataset.IncomingDatasetVersionRelations != null)
        {
            versions.AddRange(researchDataset.IncomingDatasetVersionRelations.Select(version => new Version
            { 
                DatabaseId = version.DatabaseId, 
                VersionNumber = version.VersionNumber, 
                Identifier = version.Id
            }).ToList());
        }

        if (researchDataset.OutgoingDatasetRelations != null)
        {
            foreach (var outgoingRelation in researchDataset.OutgoingDatasetRelations)
            {
                if (outgoingRelation.Type == "version")
                {
                    if (versions.All(s => s.DatabaseId != outgoingRelation.DatabaseId2))
                    {
                        versions.Add(new Version
                        {
                            DatabaseId = outgoingRelation.DatabaseId2, 
                            VersionNumber = outgoingRelation.VersionNumber2, 
                            Identifier = outgoingRelation.Id2
                        });
                    }
                
                    continue;
                }

                researchDataset.DatasetRelations ??= new List<DatasetRelation>();
                researchDataset.DatasetRelations.Add(new DatasetRelation
                { 
                    Id = outgoingRelation.Id2, 
                    Type = outgoingRelation.Type 
                });
            }
        }

        if (versions.Any())
        {
            researchDataset.VersionSet = versions;            
        }

        researchDataset.IsLatestVersion = researchDataset.VersionSet == null || researchDataset.VersionSet.Any(s =>
            s.DatabaseId == researchDataset.DatabaseId && s.VersionNumber == researchDataset.VersionSet.Max(m => m.VersionNumber));
    }

    private static void HandleResearchfiUrl(ResearchDataset researchDataset)
    {
        researchDataset.ResearchfiUrl = new ResearchfiUrl(resourceType: "research-dataset", id: researchDataset.Id);
    }
}