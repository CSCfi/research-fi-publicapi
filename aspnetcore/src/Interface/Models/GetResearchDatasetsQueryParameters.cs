using Microsoft.AspNetCore.Mvc;

namespace CSC.PublicApi.Interface.Models;

public class GetResearchDatasetsQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// Jokin kentistä nameFi, nameSv, nameEn sisältää koko tekstin.
    /// </summary>
    [FromQuery(Name = "name")]
    public string? Name { get; set; }

    /// <summary>
    /// Jokin kentistä descriptionFi, descriptionSv, descriptionEn sisältää koko tekstin.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Kentän contributors alakentän organisation alakenttä Id on täsmällleen sama kuin teksti.
    /// </summary>
    public string? OrganisationId { get; set; }

    /// <summary>
    /// Jokin kentän contributors alakentän organisation alakentistä nameFi, nameSv, nameEn sisältää koko tekstin.
    /// </summary>
    public string?  OrganisationName { get; set; }

    // <summary>
    /// Kentän contributors alakentän person alakenttä name sisältää koko tekstin.
    /// </summary>
    public string? PersonName { get; set; }

    /// <summary>
    /// Jonkin fieldOfScience kentän alakenttä fieldId on täsmälleen sama kuin teksti.
    /// </summary>
    public string? FieldOfScience { get; set; }

    /// <summary>
    /// Jonkin language kentän alakenttä code on täsmälleen sama kuin teksti.
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// Kentän accessType alakenttä code on täsmälleen sama kuin teksti.
    /// </summary>
    public string? Access { get; set; }

    /// <summary>
    /// Kentän license alakenttä code on täsmälleen sama kuin teksti.
    /// </summary>
    public string? License { get; set; }

    /// <summary>
    /// Jonkin keyword kentän alakenttä value on täsmälleen sama kuin teksti.
    /// </summary>
    public string? Keywords { get; set; }

    /// <summary>
    /// Jonkin datasetRelations kentän alakenttä id on täsmälleen sama kuin teksti.
    /// </summary>
    public string? RelatedDatasetId { get; set; }

    /// <summary>
    /// Kentän researchDataCatalog alakenttä id on täsmälleen sama kuin teksti.
    /// </summary>
    public string? ResearchDataCatalog { get; set; }
    
    /// <summary>
    /// Jos valinta on tosi palautetaan vain uusimmat version, muuten palautetaan kaikki versiot.
    /// </summary>
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