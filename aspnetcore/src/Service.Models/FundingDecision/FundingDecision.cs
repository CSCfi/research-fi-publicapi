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

    public DateTime? FundingStartDate { get; set; }

    public DateTime? FundingEndDate { get; set; }

    public List<FundingReceiver>? FundingReceivers { get; set; }

    [Number(NumberType.ScaledFloat, ScalingFactor = 100)]
    public decimal? AmountInEur { get; set; }

    public Organization? Funder { get; set; }
    
    [Keyword]
    public string? FunderProjectNumber { get; set; }

    public ReferenceData? TypeOfFunding { get; set; }

    public List<CallProgramme>? CallProgrammes { get; set; }
    
    /// <summary>
    /// Only applicable for EU funding
    /// </summary>
    public Topic? Topic { get; set; }

    /// <summary>
    /// Contains the "deepest" call programme parent, also known as FrameworkProgramme
    /// </summary>
    /// <remarks>Only applicable for EU funding.</remarks>
    public FrameworkProgramme? FrameworkProgramme { get; set; }

    public List<ReferenceData>? FieldsOfScience { get; set; }

    public List<Keyword>? Keywords { get; set; }
    
    public List<string>? IdentifiedTopics { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime Modified { get; set; }

    public string? SourceDescription { get; set; }

    [Ignore]
    public int? FunderId { get; set; }
    
    /// <summary>
    /// "Temporary" property for getting parent of decision's CallProgramme.
    /// </summary>
    [Ignore]
    public CallProgramme? CallProgrammeParent1 { get; set; }

    /// <summary>
    /// "Temporary" property for getting parent's parent of decision's CallProgramme.
    /// </summary>
    [Ignore]
    public CallProgramme? CallProgrammeParent2 { get; set; }

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
    
    [Ignore]
    public CallProgramme? CallProgramme { get; set; }
    
    /// <summary>
    /// Temporary property to create funding receivers during the in memory operations
    /// </summary>
    [Ignore]
    public List<FundingGroupPerson>? SelfFundingGroupPerson { get; set; }
    
    /// <summary>
    /// Temporary property to create funding receivers during the in memory operations
    /// </summary>
    [Ignore]
    public List<FundingGroupPerson>? ParentFundingGroupPerson { get; set; }
    
    /// <summary>
    /// Temporary property to create funding receivers during the in memory operations
    /// </summary>
    [Ignore]
    public List<OrganizationConsortium>? OrganizationConsortia { get; set; }

}