using ResearchFi.CodeList;

namespace ResearchFi.FundingDecision;

/// <summary>
/// Funding Decision
/// </summary>
public class FundingDecision
{
    /// <summary>
    /// Name of the funding decision in Finnish
    /// </summary>
    public string? NameFi { get; set; }
    
    /// <summary>
    /// Name of the funding decision in Swedish
    /// </summary>
    public string? NameSv { get; set; }
    
    /// <summary>
    /// Name of the funding decision in English
    /// </summary>
    public string? NameEn { get; set; }
    
    /// <summary>
    /// Acronym
    /// </summary>
    public string? Acronym { get; set; }
    
    /// <summary>
    /// Description of the funding decision in Finnish
    /// </summary>
    public string? DescriptionFi { get; set; }
    
    /// <summary>
    /// Description of the funding decision in Swedish
    /// </summary>
    public string? DescriptionSv { get; set; }
    
    /// <summary>
    /// Description of the funding decision in English
    /// </summary>
    public string? DescriptionEn { get; set; }
    
    /// <summary>
    /// Fundging start date
    /// </summary>
    public DateTime? FundingStartDate { get; set; }
    
   /// <summary>
    /// Funding end date
    /// </summary>
    public DateTime? FundingEndDate { get; set; }
    
    /// <summary>
    /// Funding receivers
    /// </summary>
    public List<FundingReceiver> FundingReceivers { get; set; }

    /// <summary>
    /// Funded amount in euros
    /// </summary>
    public decimal? AmountInEur { get; set; }
    
    /// <summary>
    /// Funder
    /// </summary>
    public Funder? Funder { get; set; }
    
    /// <summary>
    /// Funding type
    ///
    /// Koodisto: http://uri.suomi.fi/codelist/research/rahoitusmuoto
    /// </summary>
    public FundingType? TypeOfFunding { get; set; }
    
    /// <summary>
    /// Parts of the programme
    /// </summary>
    public List<CallProgramme>? CallProgrammes { get; set; }
    
    /// <summary>
    /// Topic
    /// </summary>
    public Topic? Topic { get; set; }

    /// <summary>
    /// Framework programme
    /// </summary>
    public FrameworkProgramme? FrameworkProgramme { get; set; }
    
    /// <summary>
    /// Funder project number
    /// </summary>
    public string? FunderProjectNumber { get; set; }
    
    /// <summary>
    /// Fields of science
    ///
    /// Koodisto: http://uri.suomi.fi/codelist/research/Tieteenala2010
    /// </summary>
    public List<FieldOfScience>? FieldsOfScience { get; set; }
    
    /// <summary>
    /// Keywords of the fields of research
    /// </summary>
    public List<Keyword>? Keywords { get; set; }
    
    /// <summary>
    /// Identified topics
    /// </summary>
    public List<string>? IdentifiedTopics { get; set; }
}