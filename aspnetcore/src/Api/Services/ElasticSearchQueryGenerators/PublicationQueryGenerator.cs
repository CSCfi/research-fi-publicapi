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

        protected override Func<SearchDescriptor<Publication>, ISearchRequest> GenerateQueryForIndex(PublicationSearchParameters searchParameters, string indexName)
        {
            return t => t
                .Index(indexName)
                .Query(q => q
                    .MultiMatch(query => query
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields("publicationName")
                        .Query(searchParameters.PublicationName)));
        }
    }
}
