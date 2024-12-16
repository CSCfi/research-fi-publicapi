namespace CSC.PublicApi.ElasticService.SearchParameters;

public class PublicationSearchParameters
{
    public string? OriginalPublicationId { get; set; }
    
    public string? Name { get; set; }

    public string? PublicationYear { get; set; }

    public string? AuthorsText { get; set; }

    public string? OrganizationId { get; set; }
    
    public string? OrganizationUnitId { get; set; }

    public string? AuthorFirstNames { get; set; }

    public string? AuthorLastName { get; set; }

    public string? AuthorOrcId { get; set; }

    public string? TypeCode { get; set; }

    public string? JournalName { get; set; }

    public string? ConferenceName { get; set; }

    public string? Issn { get; set; }
    
    public string? Isbn { get; set; }

    public string? ParentPublicationName { get; set; }

    public string? ParentPublicationPublisher { get; set; }

    public string? PublisherName { get; set; }
    
    public string? PublisherOpenAccess { get; set; }
    
    public string? JufoCode { get; set; }

    public string? JufoCodeRecorded { get; set; }
    
    public string? JufoClass { get; set; }

    public string? JufoClassRecorded { get; set; }

    public string? Doi { get; set; }
    
    public string? Keywords { get; set; }

    public string? ReportingYear { get; set; }

    public string? Status { get; set; }

    public string? CreatedFrom { get; set; }
    
    public string? CreatedTo { get; set; }

    public string? ModifiedFrom { get; set; }

    public string? ModifiedTo { get; set; }

    public bool? ShowOrganisationPartofCoPublication { get; set; }

    public bool? HideCoPublications { get; set; }
}