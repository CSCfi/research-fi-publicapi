using Api.Configuration;
using Api.Models.Publication;
using Nest;

namespace Api.Services.ElasticSearchQueryGenerators
{
    public class PublicationQueryGenerator : QueryGeneratorBase<PublicationSearchParameters, Publication>
    {
        public PublicationQueryGenerator(IndexNameSettings configuration) : base(configuration)
        {
        }

        protected override Func<QueryContainerDescriptor<Publication>, QueryContainer> GenerateQueryForIndex(PublicationSearchParameters searchParameters)
        {
            return queryContainerDescriptor => queryContainerDescriptor
                    .MultiMatch(query => query
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields("publicationName")
                        .Query(searchParameters.PublicationName));
        }
    }
}
