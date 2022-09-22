using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Publication
{
    public class PublicationSearchParameters
    {
        [FromQuery(Name = "publicationName")]
        public string? PublicationName { get; set; }
    }
}
