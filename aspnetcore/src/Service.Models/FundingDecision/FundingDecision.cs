using Nest;

namespace CSC.PublicApi.Service.Models.FundingDecision;

public class FundingDecision
{
    public string? NameFi { get; set; }

    public string? NameSv { get; set; }

    public string? NameEn { get; set; }
    
    public string? Acronym { get; set; }

    public string? DescriptionFi { get; set; }
    
    public string? DescriptionSv { get; set; }

    public string? DescriptionEn { get; set; }

    [Number(NumberType.Short)]
    public int? FundingStartYear { get; set; }

    [Number(NumberType.Short)]
    public int? FundingEndYear { get; set; }

    public DateTime? FundingEndDate { get; set; }

    public FundingGroupPerson[]? FundingGroupPerson { get; set; }

    public OrganizationConsortium[]? OrganizationConsortia { get; set; }

    [Number(NumberType.ScaledFloat, ScalingFactor = 100)]
    public decimal? AmountInEur { get; set; }

    public Funder? Funder { get; set; }
    
    public string? FunderProjectNumber { get; set; }

    public FundingType? TypeOfFunding { get; set; }

    public CallProgramme? CallProgramme { get; set; }

    /// <summary>
    /// Contains the "deepest" call programme parent, also known as FrameworkProgramme
    /// </summary>
    public FrameworkProgramme? FrameworkProgramme { get; set; }

    public FieldOfScience[]? FieldsOfScience { get; set; }

    public string[]? Keywords { get; set; }
    public string[]? IdentifiedTopics { get; set; }

    public Topic? Topic { get; set; }
    
    /// <summary>
    /// OrganizationConsortia from Suomen Akatemia decisions are mapped here temporarily during db query.
    /// They are moved to 
    /// </summary>
    [Ignore]
    public OrganizationConsortium[]? OrganizationConsortia2 { get; set; }
    
    /// <summary>
    /// "Temporary" property for getting parent of decision's CallProgramme.
    /// </summary>
    [Ignore]
    public FrameworkProgramme? CallProgrammeParent1 { get; set; }

    /// <summary>
    /// "Temporary" property for getting parent's parent of decision's CallProgramme.
    /// </summary>
    [Ignore]
    public FrameworkProgramme? CallProgrammeParent2 { get; set; }

    /// <summary>
    /// "Temporary" property for getting parent's parent's parent of decision's CallProgramme.
    /// </summary>
    [Ignore]
    public FrameworkProgramme? CallProgrammeParent3 { get; set; }

    /// <summary>
    /// "Temporary" property for getting parent's parent's parent's parent of decision's CallProgramme.
    /// </summary>
    [Ignore]
    public FrameworkProgramme? CallProgrammeParent4 { get; set; }

    /// <summary>
    /// "Temporary" property for getting parent's parent's parent's parent's parent of decision's CallProgramme.
    /// </summary>
    [Ignore]
    public FrameworkProgramme? CallProgrammeParent5 { get; set; }

    /// <summary>
    /// "Temporary" property for getting parent's parent's parent's parent's parent's parent of decision's CallProgramme.
    /// </summary>
    [Ignore]
    public FrameworkProgramme? CallProgrammeParent6 { get; set; }
}