namespace CSC.PublicApi.ElasticService;

public class SearchAfterResult
{
    public long? SearchAfter { get; }
    public int PageSize { get; }
    public long TotalResults { get; }
    public SearchAfterResult(long? searchAfter, int pageSize, long? totalResults)
    {
        SearchAfter = searchAfter;
        PageSize = pageSize;
        TotalResults = totalResults ?? 0;
    }
}