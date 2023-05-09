using ResearchFi.CodeList;

namespace ResearchFi.FundingDecision;

/// <summary>
/// Rahoituspäätös
/// </summary>
public class FundingDecision
{
    /// <summary>
    /// Hankkeen nimi
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Projektnamn
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// Name of the project
    /// </summary>
    public string? NameEn { get; set; }
    
    /// <summary>
    /// Lyhenne
    /// </summary>
    public string? Acronym { get; set; }
    
    /// <summary>
    /// Hankkeen kuvaus
    /// </summary>
    public string? DescriptionFi { get; set; }
    
    /// <summary>
    /// Projekt beskrivning
    /// </summary>
    public string? DescriptionSv { get; set; }
    
    /// <summary>
    /// Description of the project
    /// </summary>
    public string? DescriptionEn { get; set; }
    
    /// <summary>
    /// Aloitusvuosi
    /// </summary>
    public int? FundingStartYear { get; set; }
    
    /// <summary>
    /// Päättymisvuosi
    /// </summary>
    public int? FundingEndYear { get; set; }
    
    /// <summary>
    /// Päättymispäivämäärä
    /// </summary>
    public DateTime? FundingEndDate { get; set; }
    
    /// <summary>
    /// Rahoituksen saajat
    /// </summary>
    public List<FundingGroupPerson>? FundingGroupPerson { get; set; }
    
    /// <summary>
    /// Organisaatiot
    /// </summary>
    public List<OrganizationConsortium>? OrganizationConsortia { get; set; }
    
    /// <summary>
    /// Myöntösumma
    /// </summary>
    public decimal? AmountInEur { get; set; }
    
    /// <summary>
    /// Rahoittaja
    /// </summary>
    public Funder? Funder { get; set; }
    
    /// <summary>
    /// Rahoitusmuoto
    ///
    /// Koodisto: http://uri.suomi.fi/codelist/research/rahoitusmuoto
    /// </summary>
    public FundingType? TypeOfFunding { get; set; }
    
    /// <summary>
    /// Ohjelman osat
    /// </summary>
    public List<CallProgramme>? CallProgrammes { get; set; }
    
    /// <summary>
    /// Rahoitushaun aihe
    /// </summary>
    public Topic? Topic { get; set; }

    /// <summary>
    /// Puiteohjelma
    /// </summary>
    public FrameworkProgramme? FrameworkProgramme { get; set; }
    
    /// <summary>
    /// Rahoituspäätöksen numero
    /// </summary>
    public string? FunderProjectNumber { get; set; }
    
    /// <summary>
    /// Tieteenalat
    ///
    /// Koodisto: http://uri.suomi.fi/codelist/research/Tieteenala2010
    /// </summary>
    public List<FieldOfScience>? FieldsOfScience { get; set; }
    
    /// <summary>
    /// Tutkimusalojen avainsanat
    /// </summary>
    public List<Keyword>? Keywords { get; set; }
    
    /// <summary>
    /// Tunnistetut aiheet
    /// </summary>
    public List<string>? IdentifiedTopics { get; set; }
}