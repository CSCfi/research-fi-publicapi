namespace ResearchFi.Query;

/// <summary>
/// Hakuparametrit henkilöiden hakemiseen.
/// </summary>
public class GetPersonsQueryParameters
{
    /// <summary>
    /// Who's profile information permission status is queried.
    /// </summary>
    public string PersonKeyIdentifier { get; set; }

    /// <summary>
    /// Funder to whom permission is granted.
    /// </summary>
    public string GrantedController { get; set; }
}
