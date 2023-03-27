namespace CSC.PublicApi.Interface.Models.Publication;

public class LocallyReportedPublicationInformation
{
    public string? Url { get; set; }

    public string? VersionCode { get; set; }
    
    public string? LicenseCode { get; set; }

    public DateTime? EmbargoDate { get; set; }
}