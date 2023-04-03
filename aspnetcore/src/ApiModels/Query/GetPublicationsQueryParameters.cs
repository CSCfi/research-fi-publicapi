using ResearchFi.Publication;

namespace ResearchFi.Query;

/// <summary>
/// Hakuparametrit julkaisjen hakemiseen.
/// </summary>
/// <see cref="Publication"/>
public class GetPublicationsQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// Kenttä name sisältää tekstin.
    /// </summary>
    /// <see cref="Publication.Name"/>
    public string? Name { get; set; }

    /// <summary>
    /// Kenttä publicationYear on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="Publication.PublicationYear"/>
    public string? PublicationYear { get; set; }

    /// <summary>
    /// Kenttä authorsText sisältää tekstin.
    /// </summary>
    /// <see cref="Publication.AuthorsText"/>
    public string? AuthorsText { get; set; }

    /// <summary>
    /// Taulukko organizations sisältää organisaation jonka kenttä Id on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="Publication.Organizations"/>
    /// <see cref="Organization.Id"/>
    public string? OrganizationId { get; set; }

    /// <summary>
    /// Taulukko organization sisältää organisaation jolla taulukossa Units on yksikkö, jonka kenttä Id on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="Publication.Organizations"/>
    /// <see cref="Organization.Units"/>
    /// <see cref="OrganizationUnit.Id"/>
    public string? OrganizationUnitId { get; set; }

    /// <summary>
    /// Taulukko Authors sisältää tekijän, jonka firstNames kenttä sisältää tekstin.
    /// </summary>
    /// <see cref="Publication.Authors"/>
    /// <see cref="Author.FirstNames"/>
    public string? AuthorFirstNames { get; set; }

    /// <summary>
    /// Taulukko Authors sisältää tekijän, jonka lastNames kenttä sisältää tekstin.
    /// </summary>
    /// <see cref="Publication.Authors"/>
    /// <see cref="Author.LastName"/>
    public string? AuthorLastName { get; set; }

    /// <summary>
    /// Taulukko Authors sisältää tekijän, jonka orcId kenttä on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="Publication.Authors"/>
    /// <see cref="Author.Orcid"/>
    public string? AuthorOrcId { get; set; }

    /// <summary>
    /// Kenttä type:code on täsmälleen sama kuin teksti.
    ///
    /// Koodisto: http://uri.suomi.fi/codelist/research/Julkaisutyyppiluokitus
    /// </summary>
    /// <see cref="Publication.Type"/>
    public string? Type { get; set; }

    /// <summary>
    /// Kenttä journalName sisältää tekstin.
    /// </summary>
    /// <see cref="Publication.JournalName"/>
    public string? JournalName { get; set; }

    /// <summary>
    /// Kenttä conferenceName sisältää tekstin.
    /// </summary>
    /// <see cref="Publication.ConferenceName"/>
    public string? ConferenceName { get; set; }

    /// <summary>
    /// Taulukko issn sisältää arvon, joka on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="Publication.Issn"/>
    public string? Issn { get; set; }

    /// <summary>
    /// Taulukko isbn sisältää arvon, joka on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="Publication.Isbn"/>
    public string? Isbn { get; set; }

    /// <summary>
    /// Kenttä parentPublication:name sisältää tekstin. 
    /// </summary>
    /// <see cref="Publication.ParentPublication"/>
    /// <see cref="ParentPublication.Name"/>
    public string? ParentPublicationName { get; set; }

    /// <summary>
    /// Kenttä parentPublication:publisher sisältää tekstin. 
    /// </summary>
    /// <see cref="Publication.ParentPublication"/>
    /// <see cref="ParentPublication.Publisher"/>
    public string? ParentPublicationPublisher { get; set; }

    /// <summary>
    /// Kenttä publisherName sisältää tekstin.
    /// </summary>
    /// <see cref="Publication.PublisherName"/>
    public string? PublisherName { get; set; }

    /// <summary>
    /// kenttä jufoCode sisältää tekstin.
    /// </summary>
    /// <see cref="Publication.JufoCode"/>
    public string? JufoCode { get; set; }

    /// <summary>
    /// Kenttä doi on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="Publication.Doi"/>
    public string? Doi { get; set; }

    /// <summary>
    /// Taulukko keywords sisältää avainsanan, jonka kenttä value on täsmälleen sama kuin teksti. Mahdollista määritellä useampi arvo erotettuna pilkulla.
    /// </summary>
    /// <see cref="Publication.Keywords"/>
    /// <see cref="ResearchFi.Keyword.Value"/>
    public string? Keywords { get; set; }

    /// <summary>
    /// Kenttä publisherOpenAccess:code on täsmälleen sama kuin teksti.
    ///
    /// Koodisto: http://uri.suomi.fi/codelist/research/JulkaisuKanavaOA
    /// </summary>
    /// <see cref="Publication.PublisherOpenAccess"/>
    public string? PublisherOpenAccess { get; set; }

    /// <summary>
    /// Kenttä reportingYear on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="Publication.ReportingYear"/>
    public string? ReportingYear { get; set; }

    /// <summary>
    /// Kenttä status on täsmälleen sama kuin teksti.
    ///
    /// Koodisto: http://uri.suomi.fi/codelist/research/julkaisuntila
    /// </summary>
    /// <see cref="Publication.Status"/>
    public string? Status { get; set; }

    /// <summary>
    /// Luotu aikaisintaan. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    /// <see cref="Publication.Created"/>
    public string? CreatedFrom { get; set; }

    /// <summary>
    /// Luotu viimeistään. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    /// <see cref="Publication.Created"/>
    public string? CreatedTo { get; set; }

    /// <summary>
    /// Muokattu aikaisintaan. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    /// <see cref="Publication.Modified"/>
    public string? ModifiedFrom { get; set; }

    /// <summary>
    /// Muokattu viimeistään. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    /// <see cref="Publication.Modified"/>
    public string? ModifiedTo { get; set; }
}