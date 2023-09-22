namespace ResearchFi.Query;

/// <summary>
/// Sivutusta koskevat hakuehdot.
/// </summary>
public class PaginationQueryParameters
{
    private const int MaximumPageSize = 100;
    private int _pageSize = 20;
    private int _pageNumber = 1;

    /// <summary>
    /// Page number to be searched. Optional. Default value 1.
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    /// <summary>
    /// Number of results on page. Optional. Default value 20. Maximum permissible value 100. Maximum possible result set of 10,000 results.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 1 : value > MaximumPageSize ? MaximumPageSize : value;
    }
}