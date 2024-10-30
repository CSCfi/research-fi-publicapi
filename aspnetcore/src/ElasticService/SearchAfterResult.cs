namespace CSC.PublicApi.ElasticService;

public class SearchAfterResult
{
    public long? SearchAfter { get; }
    public int PageSize { get; }
    public SearchAfterResult(long? searchAfter, int pageSize)
    {
        SearchAfter = searchAfter;
        PageSize = pageSize;
    }
}