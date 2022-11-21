namespace CSC.PublicApi.Interface.Models.FundingDecision;

public class FundingDecision
{
    public string? NameFi { get; set; }
    public string? NameSv { get; set; }
    public string? NameEn { get; set; }
    public string? Acronym { get; set; }
    public string? DescriptionFi { get; set; }
    public string? DescriptionSv { get; set; }
    public string? DescriptionEn { get; set; }
    public int? FundingStartYear { get; set; }
    public int? FundingEndYear { get; set; }
    public DateTime? FundingEndDate { get; set; }
    public FundingGroupPerson[]? FundingGroupPerson { get; set; }
    public OrganizationConsortium[]? OrganizationConsortia { get; set; }
    public decimal? AmountInEur { get; set; }
    public Funder? Funder { get; set; }
    public FundingType? TypeOfFunding { get; set; }
    public CallProgramme? CallProgramme { get; set; }

    /// <summary>
    /// Contains the "deepest" call programme parent, also known as FrameworkProgramme
    /// </summary>
    public FrameworkProgramme? FrameworkProgramme { get; set; }
    public string? FunderProjectNumber { get; set; }
    public FieldOfScience[]? FieldsOfScience { get; set; }
    public string[]? Keywords { get; set; }
    public string[]? IdentifiedTopics { get; set; }
    public Topic? Topic { get; set; }
}