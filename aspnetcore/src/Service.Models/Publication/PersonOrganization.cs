using Nest;

namespace CSC.PublicApi.Service.Models.Publication;

public class AuthorOrganization
{
    [Keyword]
    public string? Id { get; set; }

    [Keyword]
    public List<string>? UnitIds { get; set; }
}