using Microsoft.AspNetCore.Mvc;

namespace CSC.PublicApi.Interface.Models;

public class GetPublicationsQueryParameters : PaginationQueryParameters
{
    [FromQuery(Name = "publicationName")]
    public string? PublicationName { get; set; }
}