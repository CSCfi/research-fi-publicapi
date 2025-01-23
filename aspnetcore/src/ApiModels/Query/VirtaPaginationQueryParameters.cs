namespace ResearchFi.Query;

/// <summary>
/// Sivutusta koskevat hakuehdot.
/// </summary>
public class VirtaPaginationQueryParameters
{
   // private const int MaximumPageSize = 10;
    private int _pageSize = 100;
    private int _pageNumber = 1;

    /// <summary>
    /// Sivunumero. Valinnainen. Määrittää ohitettavien sivujen määrän. Oletusarvo on 1 (näytetään ensimmäinen sivu).
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    /// <summary>
    /// Sivukoko. Valinnainen. Määrittää kuinka monta tulosta vastauksessa palautetaan. Oletusarvo on 100.  
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 1 : value ;
    }
}