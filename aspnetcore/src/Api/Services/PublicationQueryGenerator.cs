using Api.Models;
using Nest;

namespace Api.Services
{
    public class PublicationQueryGenerator : IQueryGenerator<Publication>
    {
        public Func<SearchDescriptor<Publication>, ISearchRequest> GenerateQuery(string searchText)
        {
            return t => t
                .Index("publication")
                .Query(q => q
                    .MultiMatch(query => query
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields("publicationName")
                        .Query(searchText)));
        }

    }
}
