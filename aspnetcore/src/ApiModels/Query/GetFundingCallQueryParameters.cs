using ResearchFi.FundingCall;

namespace ResearchFi.Query;

/// <summary>
/// Hakuparametrit rahoitushakujen filtteröimiseen.
/// </summary>
/// <see cref="FundingCall"/>
public class GetFundingCallQueryParameters : PaginationQueryParameters
{
    /// <summary>
    /// Jokin kentistä nameFi, nameSV, nameEn sisältää koko tekstin.
    /// </summary>
    /// <see cref="FundingCall.NameFi"/>
    /// <see cref="FundingCall.NameSv"/>
    /// <see cref="FundingCall.NameEn"/>
    public string? Name { get; set; }

    /// <summary>
    /// Jokin kentän foundation alikentistä nameFi, nameSV, nameEn sisältää koko tekstin.
    /// </summary>
    /// <see cref="FundingCall.Foundations"/>
    /// <see cref="Foundation.NameFi"/>
    /// <see cref="Foundation.NameSv"/>
    /// <see cref="Foundation.NameEn"/>
    public string? FoundationName { get; set; }

    /// <summary>
    /// Kenttä foundation.foundationBusinessId on täsmälleen sama kuin teksti.
    /// </summary>
    /// <see cref="FundingCall.Foundations"/>
    /// <see cref="Foundation.BusinessId"/>
    public string? FoundationBusinessId { get; set; }

    /// <summary>
    /// Kenttä categories.codeValue on täsmälleen sama kuin teksti.
    /// 
    /// Koodisto: http://uri.suomi.fi/codelist/research/auroran_alat
    /// </summary>
    /// <see cref="FundingCall.Categories"/>
    public string? Category { get; set; }

    /// <summary>
    /// Haku alkaa aikaisintaan. CallProgrammeOpenDate oltava sama tai suurempi kuin pvm. Päivämäärä muodossa vvvv-kk-pp.
    /// </summary>
    /// <see cref="FundingCall.CallProgrammeOpenDate"/>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Haku päättyy viimeistään. CallProgrammeDueDate oltava sama tai pienempi kuin pvm. Päivämäärä muodossa vvvv-kk-pp.
    /// </summary>
    /// <see cref="FundingCall.CallProgrammeDueDate"/>
    public DateTime? DateTo { get; set; }
}