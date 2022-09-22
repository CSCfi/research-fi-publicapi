namespace Api.Models;

public class SearchResult<T> : List<T> where T : class
{
    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalPages => (int)Math.Ceiling(TotalResults / (double)PageSize);

    public long TotalResults { get; }

    public SearchResult(IEnumerable<T> results, int pageNumber, int pageSize, long? totalResults)
    {
        AddRange(results);

        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalResults = totalResults ?? 0;
    }
}