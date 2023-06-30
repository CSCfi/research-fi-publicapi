using ResearchFi.Publication;

namespace ResearchFi.Query;

/// <summary>
/// Query parameters for searching publications.
/// </summary>
/// <see cref="Publication"/>
public class GetPublicationsQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// The field name contains text.
    /// </summary>
    /// <see cref="Publication.Name"/>
    public string? Name { get; set; }

    /// <summary>
    /// The field publicationYear is exactly equal to the text.
    /// </summary>
    /// <see cref="Publication.PublicationYear"/>
    public string? PublicationYear { get; set; }

    /// <summary>
    /// The field authorsText contains text.
    /// </summary>
    /// <see cref="Publication.AuthorsText"/>
    public string? AuthorsText { get; set; }

    /// <summary>
    /// Table organizations contains an organization whose field Id is exactly equal to the text.
    /// </summary>
    /// <see cref="Publication.Organizations"/>
    /// <see cref="Organization.Id"/>
    public string? OrganizationId { get; set; }

    /// <summary>
    /// Table organization contains an organization in which the table Units has a unit whose field Id is exactly equal to the text.
    /// </summary>
    /// <see cref="Publication.Organizations"/>
    /// <see cref="Organization.Units"/>
    /// <see cref="OrganizationUnit.Id"/>
    public string? OrganizationUnitId { get; set; }

    /// <summary>
    /// Table Authors contains an author, whose firstNames field contains the text.
    /// </summary>
    /// <see cref="Publication.Authors"/>
    /// <see cref="Author.FirstNames"/>
    public string? AuthorFirstNames { get; set; }

    /// <summary>
    /// Table Authors contains an author, whose lastNames field contains the text.
    /// </summary>
    /// <see cref="Publication.Authors"/>
    /// <see cref="Author.LastName"/>
    public string? AuthorLastName { get; set; }

    /// <summary>
    /// Table Authors contains an author whose orcId field is exactly equal to the text.
    /// </summary>
    /// <see cref="Publication.Authors"/>
    /// <see cref="Author.Orcid"/>
    public string? AuthorOrcId { get; set; }

    /// <summary>
    /// The field type:code is exactly equal to the text.
    ///
    /// Code: http://uri.suomi.fi/codelist/research/Julkaisutyyppiluokitus
    /// </summary>
    /// <see cref="Publication.Type"/>
    public string? Type { get; set; }

    /// <summary>
    /// The field journalName contains text.
    /// </summary>
    /// <see cref="Publication.JournalName"/>
    public string? JournalName { get; set; }

    /// <summary>
    /// The field conferenceName contains text.
    /// </summary>
    /// <see cref="Publication.ConferenceName"/>
    public string? ConferenceName { get; set; }

    /// <summary>
    /// Table issn contains a value that is exactly equal to the text.
    /// </summary>
    /// <see cref="Publication.Issn"/>
    public string? Issn { get; set; }

    /// <summary>
    /// Table isbn contains a value that is exactly equal to the text.
    /// </summary>
    /// <see cref="Publication.Isbn"/>
    public string? Isbn { get; set; }

    /// <summary>
    /// The field parentPublication:name contains text.
    /// </summary>
    /// <see cref="Publication.ParentPublication"/>
    /// <see cref="ParentPublication.Name"/>
    public string? ParentPublicationName { get; set; }

    /// <summary>
    /// The field parentPublication:publisher contains text.
    /// </summary>
    /// <see cref="Publication.ParentPublication"/>
    /// <see cref="ParentPublication.Publisher"/>
    public string? ParentPublicationPublisher { get; set; }

    /// <summary>
    /// The field publisherName contains text.
    /// </summary>
    /// <see cref="Publication.PublisherName"/>
    public string? PublisherName { get; set; }

    /// <summary>
    /// The field jufocode contains text.
    /// </summary>
    /// <see cref="Publication.JufoCode"/>
    public string? JufoCode { get; set; }

    /// <summary>
    /// The field doi is exactly equal to the text.
    /// </summary>
    /// <see cref="Publication.Doi"/>
    public string? Doi { get; set; }

    /// <summary>
    /// Table keywords contains a keyword whose field value is exactly equal to the text. It is possible to define several values separated by a comma.
    /// </summary>
    /// <see cref="Publication.Keywords"/>
    /// <see cref="ResearchFi.Keyword.Value"/>
    public string? Keywords { get; set; }

    /// <summary>
    /// The field publisherOpenAccess:code is exactly equal to the text.
    ///
    /// Code: http://uri.suomi.fi/codelist/research/JulkaisuKanavaOA
    /// </summary>
    /// <see cref="Publication.PublisherOpenAccess"/>
    public string? PublisherOpenAccess { get; set; }

    /// <summary>
    /// The field reportingYear is exactly equal to the text.
    /// </summary>
    /// <see cref="Publication.ReportingYear"/>
    public string? ReportingYear { get; set; }

    /// <summary>
    /// The field status is exactly equal to the text.
    ///
    /// Code: http://uri.suomi.fi/codelist/research/julkaisuntila
    /// </summary>
    /// <see cref="Publication.Status"/>
    public string? Status { get; set; }

    /// <summary>
    /// Created at the earliest. Date format yyyy-mm-dd
    /// </summary>
    /// <see cref="Publication.Created"/>
    public string? CreatedFrom { get; set; }

    /// <summary>
    /// Created at the latest. Date format yyyy-mm-dd
    /// </summary>
    /// <see cref="Publication.Created"/>
    public string? CreatedTo { get; set; }

    /// <summary>
    /// Edited at the earliest. Date format yyyy-mm-dd
    /// </summary>
    /// <see cref="Publication.Modified"/>
    public string? ModifiedFrom { get; set; }

    /// <summary>
    /// Edited at the latest. Date format yyyy-mm-dd
    /// </summary>
    /// <see cref="Publication.Modified"/>
    public string? ModifiedTo { get; set; }
}