namespace CSC.PublicApi.ElasticService.SearchParameters;

/// <summary>
/// Parameters for searching funding calls from Elastic Search.
/// </summary>
public class FundingCallSearchParameters
{
    public string? Name { get; set; }

    public string? FoundationName { get; set; }

    public string? FoundationBusinessId { get; set; }

    public string? CategoryCode { get; set; }
    
    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }
}