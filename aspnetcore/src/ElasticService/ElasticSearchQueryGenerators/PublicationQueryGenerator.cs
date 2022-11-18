using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models.Publication;
using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

public class PublicationQueryGenerator : QueryGeneratorBase<PublicationSearchParameters, Publication>
{
    public PublicationQueryGenerator(IndexNameSettings configuration) : base(configuration)
    {
    }

    protected override Func<QueryContainerDescriptor<Publication>, QueryContainer> GenerateQueryForIndex(PublicationSearchParameters parameters)
    {
        return queryContainerDescriptor => queryContainerDescriptor
            .MultiMatch(query => query
                .Type(TextQueryType.PhrasePrefix)
                .Fields("publicationName")
                .Query(parameters.PublicationName));
    }
}