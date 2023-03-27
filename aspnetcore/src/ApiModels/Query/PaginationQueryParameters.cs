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
    /// Haettavan sivun numero. Oletusarvo 1.
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    /// <summary>
    /// Tulosten määrä sivulla. Oletusarvo 20. Suurin sallittu arvo 100. Suurin mahdollinen tulosjoukko 10 000 tulosta.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 1 : value > MaximumPageSize ? MaximumPageSize : value;
    }
}