namespace ResearchFi.Query;

/// <summary>
/// Vientiin liittyvät tiedot.
/// </summary>
public class SearchAfterQueryParameters
{
    private const int MaximumPageSize = 500;
    private int _pageSize = 20;
    private long? _nextPageToken = null;

    /// <summary>
    /// Number of results on page. Optional. Default value 20. Maximum permissible value 500.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 20 : (value > MaximumPageSize ? MaximumPageSize : value);
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