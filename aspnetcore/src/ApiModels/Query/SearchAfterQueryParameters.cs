namespace ResearchFi.Query;

/// <summary>
/// Vientiin liittyvät tiedot.
/// </summary>
public class SearchAfterQueryParameters
{
    private const int DefaultPageSize = 50;
    private const int MaximumPageSize = 1000;
    private int _pageSize = DefaultPageSize;
    private long? _nextPageToken = null;

    /// <summary>
    /// Number of results on page. Optional. Default value 50. Maximum permissible value 1000.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? DefaultPageSize : (value > MaximumPageSize ? MaximumPageSize : value);
    }

    /// <summary>
    /// Value from previous query response header "x-next-page-token". Leave empty in the first query.
    /// </summary>
    public long? NextPageToken
    {
        get => _nextPageToken;
        set => _nextPageToken = value;
    }
}