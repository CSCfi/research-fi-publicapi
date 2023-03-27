namespace CSC.PublicApi.Interface.Models;

public class GetPublicationsQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// Kenttä name sisältää tekstin.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Kenttä publicationYear on täsmälleen sama kuin teksti.
    /// </summary>
    public string? PublicationYear { get; set; }

    /// <summary>
    /// Kenttä authorsText sisältää tekstin.
    /// </summary>
    public string? AuthorsText { get; set; }

    /// <summary>
    /// Taulukko organization sisältää organisaation jonka kenttä Id on täsmälleen sama kuin teksti.
    /// </summary>
    public string? OrganizationId { get; set; }
    
    /// <summary>
    /// Taulukko organization sisältää organisaation jolla taulukossa Units on yksikkö, jonka kenttä Id on täsmälleen sama kuin teksti.
    /// </summary>
    public string? OrganizationUnitId { get; set; }

    /// <summary>
    /// Taulukko Authors sisältää tekijän, jonka firstNames kenttä sisältää tekstin.
    /// </summary>
    public string? AuthorFirstNames { get; set; }

    /// <summary>
    /// Taulukko Authors sisältää tekijän, jonka lastNames kenttä sisältää tekstin.
    /// </summary>
    public string? AuthorLastName { get; set; }

    /// <summary>
    /// Taulukko Authors sisältää tekijän, jonka orcId kenttä on täsmälleen sama kuin teksti.
    /// </summary>
    public string? AuthorOrcId { get; set; }

    /// <summary>
    /// Kenttä typeCode on täsmälleen sama kuin teksti.
    /// </summary>
    public string? TypeCode { get; set; }

    /// <summary>
    /// Kenttä journalName sisältää tekstin.
    /// </summary>
    public string? JournalName { get; set; }

    /// <summary>
    /// Kenttä conferenceName sisältää tekstin.
    /// </summary>
    public string? ConferenceName { get; set; }

    /// <summary>
    /// Taulukko issn sisältää arvon, joka on täsmälleen sama kuin teksti.
    /// </summary>
    public string? Issn { get; set; }
    
    /// <summary>
    /// Taulukko isbn sisältää arvon, joka on täsmälleen sama kuin teksti.
    /// </summary>
    public string? Isbn { get; set; }

    /// <summary>
    /// Kentän parentPublication alakenttä name sisältää tekstin. 
    /// </summary>
    public string? ParentPublicationName { get; set; }

    /// <summary>
    /// Kentän parentPublication alakenttä publisher sisältää tekstin. 
    /// </summary>
    public string? ParentPublicationPublisher { get; set; }

    /// <summary>
    /// Kenttä publisherName sisältää tekstin.
    /// </summary>
    public string? PublisherName { get; set; }

    /// <summary>
    /// kenttä jufoCode sisältää tekstin.
    /// </summary>
    public string? JufoCode { get; set; }

    /// <summary>
    /// Kenttä doi on täsmälleen sama kuin teksti.
    /// </summary>
    public string? Doi { get; set; }

    /// <summary>
    /// Taulukko keywords sisältää avainsanan, jonka kenttä value on täsmälleen sama kuin teksti. 
    /// </summary>
    public string? Keywords { get; set; }

    /// <summary>
    /// Kenttä reportingYear on täsmälleen sama kuin teksti.
    /// </summary>
    public string? ReportingYear { get; set; }

    /// <summary>
    /// Kenttä statusCode on täsmälleen sama kuin teksti.
    /// </summary>
    public string? StatusCode { get; set; }

    /// <summary>
    /// Luotu aikaisintaan. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    public string? CreatedFrom { get; set; }
    
    /// <summary>
    /// Luotu viimeistään. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    public string? CreatedTo { get; set; }

    /// <summary>
    /// Muokattu aikaisintaan. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    public string? ModifiedFrom { get; set; }

    /// <summary>
    /// Muokattu viimeistään. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    public string? ModifiedTo { get; set; }
}