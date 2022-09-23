using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models.ResearchDataset;
using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

public class ResearchDatasetQueryGenerator : QueryGeneratorBase<ResearchDatasetSearchParameters, ResearchDataset>
{
    public ResearchDatasetQueryGenerator(IndexNameSettings configuration) : base(configuration)
    {
    }

    protected override Func<QueryContainerDescriptor<ResearchDataset>, QueryContainer> GenerateQueryForIndex(ResearchDatasetSearchParameters parameters)
    {
        var subQueries = new List<Func<QueryContainerDescriptor<ResearchDataset>, QueryContainer>>();
        var filters = new List<Func<QueryContainerDescriptor<ResearchDataset>, QueryContainer>>();

        // When searching with Name, search from Fi,Sv,En names.
        if (!string.IsNullOrWhiteSpace(parameters.Name))
        {
            subQueries.Add(t => t.MultiMatch(query => query
                .Type(TextQueryType.PhrasePrefix)
                .Fields("nameFi, nameSv, nameEn")
                .Query(parameters.Name)));
        }

        return queryDescriptor => queryDescriptor
            .Bool(boolDescriptor => boolDescriptor
                .Must(subQueries)
                .Filter(filters)
            );
    }
}