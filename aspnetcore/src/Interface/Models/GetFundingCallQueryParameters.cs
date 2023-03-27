using Microsoft.AspNetCore.Mvc;

namespace CSC.PublicApi.Interface.Models;

/// <summary>
/// HTTP Get query parameters for searching funding calls. 
/// </summary>
/// <see cref="CSC.PublicApi.Interface.Models.FundingCall.FundingCall"/>
public class GetFundingCallQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// Jokin kentistä nameFi, nameSV, nameEn sisältää koko tekstin.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Jokin kentän foundation alikentistä nameFi, nameSV, nameEn sisältää koko tekstin.
    /// </summary>
    public string? FoundationName { get; set; }

    /// <summary>
    /// Kenttä foundation.foundationBusinessId on täsmälleen sama kuin teksti.
    /// </summary>
    public string? FoundationBusinessId { get; set; }

    /// <summary>
    /// Kenttä categories.codeValue on täsmälleen sama kuin teksti.
    /// </summary>
    public string? CategoryCode { get; set; }

    /// <summary>
    /// Haku alkaa aikaisintaan. CallProgrammeOpenDate oltava sama tai suurempi kuin pvm. Päivämäärä muodossa vvvv-kk-pp.
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Haku päättyy viimeistään. CallProgrammeDueDate oltava sama tai pienempi kuin pvm. Päivämäärä muodossa vvvv-kk-pp.
    /// </summary>
    public DateTime? DateTo { get; set; }
}