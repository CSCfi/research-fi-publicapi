using ResearchFi.FundingCall;

namespace ResearchFi.Query;

/// <summary>
/// Query parameters for searching funding calls.
/// </summary>
/// <see cref="FundingCall"/>
public class GetFundingCallQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// One of the fields nameFi, nameSV, nameEn contains the full text.
    /// </summary>
    /// <see cref="FundingCall.NameFi"/>
    /// <see cref="FundingCall.NameSv"/>
    /// <see cref="FundingCall.NameEn"/>
    public string? Name { get; set; }

    /// <summary>
    /// One of the field foundation subfields nameFi, nameSV, nameEn contains the full text.
    /// </summary>
    /// <see cref="FundingCall.Foundations"/>
    /// <see cref="Foundation.NameFi"/>
    /// <see cref="Foundation.NameSv"/>
    /// <see cref="Foundation.NameEn"/>
    public string? FoundationName { get; set; }

    /// <summary>
    /// The field foundation.foundationBusinessId is exactly equal to the text.
    /// </summary>
    /// <see cref="FundingCall.Foundations"/>
    /// <see cref="Foundation.BusinessId"/>
    public string? FoundationBusinessId { get; set; }

    /// <summary>
    /// The field categories.codeValue is exactly equal to the text. 
    /// 
    /// Code: http://uri.suomi.fi/codelist/research/auroran_alat
    /// </summary>
    /// <see cref="FundingCall.Categories"/>
    public string? Category { get; set; }

    /// <summary>
    /// The search will start at the earliest. CallProgrammeOpenDate must be the equal or greater than the date. Date format yyyy-mm-dd.
    /// </summary>
    /// <see cref="FundingCall.CallProgrammeOpenDate"/>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// The search will end at the latest. CallProgrammeOpenDate must be the equal or less than the date. Date format yyyy-mm-dd.
    /// </summary>
    /// <see cref="FundingCall.CallProgrammeDueDate"/>
    public DateTime? DateTo { get; set; }
}