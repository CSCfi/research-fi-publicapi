using Api.Models;
using Nest;

namespace Api.Services
{
    public class PublicationQueryGenerator : IQueryGenerator<PublicationSearchParameters, Publication>
    {
        public Func<SearchDescriptor<Publication>, ISearchRequest> GenerateQuery(PublicationSearchParameters parameters)
        {
            return t => t
                .Index("publication")
                .Query(q => q
                    .MultiMatch(query => query
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields("publicationName")
                        .Query(parameters.PublicationName)));
        }

    }
}
