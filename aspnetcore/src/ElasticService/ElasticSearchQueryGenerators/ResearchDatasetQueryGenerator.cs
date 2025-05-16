using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.ResearchDataset;
using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

public class ResearchDatasetQueryGenerator : QueryGeneratorBase<ResearchDatasetSearchParameters, ResearchDataset>
{
    public ResearchDatasetQueryGenerator(IndexNameSettings configuration) : base(configuration)
    {
    }

    protected override Func<QueryContainerDescriptor<ResearchDataset>, QueryContainer> GenerateQueryForSearch(
        ResearchDatasetSearchParameters parameters)
    {
        var subQueries = CreateSubQueries(parameters);
        var filters = CreateFilters(parameters);

        return queryDescriptor => queryDescriptor
            .Bool(boolDescriptor => boolDescriptor
                .Must(subQueries)
                .Filter(filters)
            );
    }

    protected override Func<QueryContainerDescriptor<ResearchDataset>, QueryContainer> GenerateQueryForSingle(string id)
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<Func<QueryContainerDescriptor<ResearchDataset>, QueryContainer>> CreateSubQueries(
        ResearchDatasetSearchParameters parameters)
    {
        var subQueries = new List<Func<QueryContainerDescriptor<ResearchDataset>, QueryContainer>>();

        // When searching with Name, search from Fi,Sv,En names.
        if (!string.IsNullOrWhiteSpace(parameters.Name))
        {
            subQueries.Add(t => t.MultiMatch(query => query
                .Type(TextQueryType.PhrasePrefix)
                .Fields(r => r
                    .Field(f => f.NameFi)
                    .Field(f => f.NameSv)
                    .Field(f => f.NameEn))
                .Query(parameters.Name)));
        }

        // When searching with Description, search from Fi,Sv and En descriptions.
        if (!string.IsNullOrWhiteSpace(parameters.Description))
        {
            subQueries.Add(t => t.MultiMatch(query => query
                .Type(TextQueryType.PhrasePrefix)
                .Fields(r => r
                    .Field(f => f.DescriptionFi)
                    .Field(f => f.DescriptionSv)
                    .Field(f => f.DescriptionEn))
                .Query(parameters.Description)));
        }

        if (parameters.Keywords is not null)
        {
            subQueries.Add(t =>
                t.Match(m =>
                    m.Field(f => f.Keywords.Suffix(nameof(Keyword.Value)))
                        .Query(parameters.Keywords)
                ));
        }

        if (parameters.SubjectHeadings is not null)
        {
            subQueries.Add(t =>
                t.Match(m =>
                    m.Field(f => f.SubjectHeadings.Suffix(nameof(Keyword.Value)))
                        .Query(parameters.SubjectHeadings)
                ));
        }

        if (parameters.OrganizationName is not null)
        {
            subQueries.Add(t =>
                t.MultiMatch(m =>
                    m.Fields(r => r
                            .Field(f => f.Contributors.Suffix(nameof(Contributor.Organization))
                                .Suffix(nameof(Organization.NameFi)))
                            .Field(f => f.Contributors.Suffix(nameof(Contributor.Organization))
                                .Suffix(nameof(Organization.NameSv)))
                            .Field(f => f.Contributors.Suffix(nameof(Contributor.Organization))
                                .Suffix(nameof(Organization.NameEn)))
                            .Field(f => f.Contributors.Suffix(nameof(Contributor.Organization))
                                .Suffix(nameof(Organization.NameVariants))))
                        .Query(parameters.OrganizationName)
                ));
        }

        if (parameters.PersonName is not null)
        {
            subQueries.Add(t =>
                t.Match(m =>
                    m.Field(f => f.Contributors.Suffix(nameof(Contributor.Person))
                            .Suffix(nameof(Person.Name)))
                        .Query(parameters.PersonName)
                ));
        }

        return subQueries;
    }

    private static IEnumerable<Func<QueryContainerDescriptor<ResearchDataset>, QueryContainer>> CreateFilters(
        ResearchDatasetSearchParameters parameters)
    {
        var filters = new List<Func<QueryContainerDescriptor<ResearchDataset>, QueryContainer>>();

        // IsLatestVersion returns only latest versions, otherwise return all versions.
        if (parameters.IsLatestVersion is not null && parameters.IsLatestVersion.Value)
        {
            filters.Add(t =>
                t.Term(s => s.Field(f => f.IsLatestVersion)
                    .Value(true)));
        }

        if (parameters.FieldOfScience is not null)
        {
            filters.Add(t =>
                t.Terms(term =>
                    term.Field(f =>
                            f.FieldsOfScience.Suffix(nameof(ReferenceData.Code)))
                        .Terms(parameters.FieldOfScience)
                ));
        }

        if (parameters.Language is not null)
        {
            filters.Add(t =>
                t.Terms(term =>
                    term.Field(f => f.Languages.Suffix(nameof(ReferenceData.Code)))
                        .Terms(parameters.Language)
                ));
        }

        if (parameters.Access is not null)
        {
            filters.Add(t =>
                t.Term(term =>
                    term.Field(f => f.AccessType.Suffix(nameof(ReferenceData.Code)))
                        .Value(parameters.Access)
                ));
        }

        if (parameters.License is not null)
        {
            filters.Add(t =>
                t.Term(term =>
                    term.Field(f => f.License.Suffix(nameof(ReferenceData.Code)))
                        .Value(parameters.License)
                ));
        }

        if (parameters.RelatedDatasetId is not null)
        {
            filters.Add(t =>
                t.Term(term =>
                    term.Field(f => f.DatasetRelations.Suffix(nameof(DatasetRelation.Id)))
                        .Value(parameters.RelatedDatasetId)
                ));
        }

        if (parameters.OrganizationId is not null)
        {
            filters.Add(t =>
                t.Term(term =>
                    term.Field(f =>
                            f.Contributors.Suffix(nameof(Contributor.Organization)).Suffix(nameof(Organization.Id)))
                        .Value(parameters.OrganizationId)
                ));
        }

        if (parameters.ResearchDataCatalog is not null)
        {
            filters.Add(t =>
                t.Term(term =>
                    term.Field(f =>
                            f.ResearchDataCatalog.Suffix(nameof(ResearchDataCatalog.Id)))
                        .Value(parameters.ResearchDataCatalog)
                ));
        }

        if (parameters.DateFrom is not null)
        {
            filters.Add(x => x
                .Bool(b => b
                    .Should(s =>
                    (
                        s.DateRange(r => r
                            .Field(f => f.Created)
                            .GreaterThanOrEquals(parameters.DateFrom))
                    ))));
        }

        if (parameters.DateTo is not null)
        {
            filters.Add(x => x
                .Bool(b => b
                    .Should(s =>
                    (
                        s.DateRange(r => r
                            .Field(f => f.Created)
                            .LessThanOrEquals(parameters.DateTo))
                    ))));
        }

        return filters;
    }

    protected override Func<SortDescriptor<ResearchDataset>, IPromise<IList<ISort>>> GenerateSortForSearch(ResearchDatasetSearchParameters parameters)
    {
        // Sort research datasets
        return sortDescriptor => sortDescriptor
            .Field(f => f.Created, SortOrder.Descending)
            .Field(f => f.ExportSortId, SortOrder.Ascending); // DO NOT REMOVE, prevents duplicates in the result set
    }
}