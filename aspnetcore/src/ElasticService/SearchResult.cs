namespace CSC.PublicApi.ElasticService;

public class SearchResult
{
    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalPages => (int)Math.Ceiling(TotalResults / (double)PageSize);

    public long TotalResults { get; }

    public SearchResult(int pageNumber, int pageSize, long? totalResults)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalResults = totalResults ?? 0;
    }
}