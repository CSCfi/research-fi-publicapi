using Microsoft.AspNetCore.Mvc;

namespace CSC.PublicApi.Interface.Models;

public class GetResearchDatasetsQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// Jokin kentistä nameFi, nameSv, nameEn sisältää tekstin.
    /// </summary>
    [FromQuery(Name = "name")]
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? OrganisationId { get; set; }

    public string?  OrganisationName { get; set; }

    public string? PersonName { get; set; }

    public string? FieldOfScience { get; set; }

    public string? Language { get; set; }

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