namespace CSC.PublicApi.Service.Models;

public class OrganizationPartOfCoPublication
{

    public string? Id { get; set; }
    public string? OriginalPublicationId { get; set; }
    public OrganizationPartOfPublication_Organization? Organization { get; set; }
}