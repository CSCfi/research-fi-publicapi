namespace CSC.PublicApi.Service.Models.Publication;

public class LocallyReportedPublicationInformation
{
    public string? Url { get; set; }

    public ReferenceData? Version { get; set; }
    
    public ReferenceData? License { get; set; }

    public DateTime? EmbargoDate { get; set; }
}