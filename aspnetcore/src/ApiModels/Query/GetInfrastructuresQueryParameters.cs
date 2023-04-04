namespace ResearchFi.Query;

/// <summary>
/// Hakuparametrit infrastruktuurien hakemiseen.
/// </summary>
public class GetInfrastructuresQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// Infrastruktuurin nimi.
    /// </summary>
    public string? Name { get; set; }
}