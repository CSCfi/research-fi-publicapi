namespace CSC.PublicApi.ElasticService;

public class SearchAfterResult
{
    public long? SearchAfter { get; }
    public int PageSize { get; }

    public long? TotalResults { get; set; }
    public SearchAfterResult(long? searchAfter, int pageSize, long? totalResults = null)
    {
        SearchAfter = searchAfter;
        PageSize = pageSize;
        TotalResults = totalResults;
    }
}