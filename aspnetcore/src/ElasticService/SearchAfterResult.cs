namespace CSC.PublicApi.ElasticService;

public class SearchAfterResult
{
    public string? SearchAfter { get; }
    public int PageSize { get; }
    public SearchAfterResult(string? searchAfter, int pageSize)
    {
        SearchAfter = searchAfter;
        PageSize = pageSize;
    }
}