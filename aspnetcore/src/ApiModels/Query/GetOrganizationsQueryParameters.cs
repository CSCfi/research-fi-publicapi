namespace ResearchFi.Query;

/// <summary>
/// Hakuparametrit organisaatioiden hakemiseen.
/// </summary>
public class GetOrganizationsQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// Organisaation nimi.
    /// </summary>
    public string? Name { get; set; }
}