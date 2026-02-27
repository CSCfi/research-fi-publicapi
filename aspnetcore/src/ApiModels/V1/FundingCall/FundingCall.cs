using ResearchFi.CodeList;

namespace ResearchFi.FundingCall;

/// <summary>
/// Funding Call
/// </summary>
public class FundingCall
{
    /// <summary>
    /// Name of the funding call in Finnish
    /// </summary>
    public string? NameFi { get; set; }

    /// <summary>
    /// Name of the funding call in Swedish
    /// </summary>
    public string? NameSv { get; set; }

    /// <summary>
    /// Name of the funding call in English
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Description of the funding call in Finnish
    /// </summary>
    public string? DescriptionFi { get; set; }

    /// <summary>
    ///  Description of the funding call in Swedish
    /// </summary>
    public string? DescriptionSv { get; set; }

    /// <summary>
    /// Description of the funding call in English
    /// </summary>
    public string? DescriptionEn { get; set; }

    /// <summary>
    /// Application terms in Finnish
    /// </summary>
    public string? ApplicationTermsFi { get; set; }

    /// <summary>
    /// Application terms in Swedish
    /// </summary>
    public string? ApplicationTermsSv { get; set; }

    /// <summary>
    /// Application terms in English
    /// </summary>
    public string? ApplicationTermsEn { get; set; }

    /// <summary>
    /// Continous application
    /// </summary>
    public bool ContinuousApplication { get; set; }

    /// <summary>
    /// Call programme open date
    /// </summary>
    public DateTime? CallProgrammeOpenDate { get; set; }

    /// <summary>
    /// Call programme due date
    /// </summary>
    public DateTime? CallProgrammeDueDate { get; set; }

    /// <summary>
    /// Webpage of the funding call in Finnish
    /// </summary>
    public string? ApplicationUrlFi { get; set; }

    /// <summary>
    /// Webpage of the funding call in Swedish
    /// </summary>
    public string? ApplicationUrlSv { get; set; }

    /// <summary>
    /// Webpage of the funding call in English
    /// </summary>
    public string? ApplicationUrlEn { get; set; }

    /// <summary>
    /// Funders
    /// </summary>
    public List<Foundation>? Foundations { get; set; }

    /// <summary>
    /// Categories of the call
    /// 
    /// Koodisto: https://uri.suomi.fi/codelist/research/auroran_alat
    /// </summary>
    public List<Category>? Categories { get; set; }

    /// <summary>
    /// Contact information
    /// </summary>
    public string? ContactInformation { get; set; }

    /// <summary>
    /// Researchfi portal URL
    /// </summary>
    public ResearchfiUrl? ResearchfiUrl { get; set; }
}