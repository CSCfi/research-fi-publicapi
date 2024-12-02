namespace ResearchFi.Query;

/// <summary>
/// Sivutusta koskevat hakuehdot.
/// </summary>
public class VirtaPaginationQueryParameters
{
   // private const int MaximumPageSize = 10;
    private int _pageSize = 10000000;
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
    /// Number of results on page. Optional. Default value 1000000.  
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 1 : value ;
    }
}