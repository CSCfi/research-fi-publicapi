namespace CSC.PublicApi.ElasticService.SearchParameters;

public class ResearchDatasetSearchParameters
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? OrganizationId { get; set; }

    public string? OrganizationName { get; set; }

    public string? PersonName { get; set; }

    public List<string>? FieldOfScience { get; set; }

    public List<string>? Language { get; set; }

    public string? Access { get; set; }

    public string? License { get; set; }

    public string? Keywords { get; set; }

    public string? RelatedDatasetId { get; set; }

    public string? ResearchDataCatalog { get; set; }
    
    public bool? IsLatestVersion { get; set; }
    
    /// <summary>
    /// Luotu aikaisintaan. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Luotu viimeistään. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    public DateTime? DateTo { get; set; }
}