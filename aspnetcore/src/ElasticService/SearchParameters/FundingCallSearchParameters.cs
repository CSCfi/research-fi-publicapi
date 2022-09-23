namespace CSC.PublicApi.ElasticService.SearchParameters;

public class FundingCallSearchParameters
{
    public string? Name { get; set; }

    public string? FoundationName { get; set; }

    public string? FoundationBusinessId { get; set; }

    public string? CategoryCode { get; set; }

    /// <summary>
    /// Haku alkaa aikaisintaan. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Haku päättyy viimeistään. Päivämäärä muodossa vvvv-kk-pp
    /// </summary>
    public DateTime? DateTo { get; set; }
}