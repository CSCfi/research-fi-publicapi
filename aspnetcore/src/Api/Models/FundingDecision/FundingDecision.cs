using Nest;

namespace Api.Models.FundingDecision
{
    public class FundingDecision
    {
        public string? NameFi { get; set; }

        public string? NameSv { get; set; }

        public string? NameEn { get; set; }
        public string? Acronym { get; set; }

        public string? DescriptionFi { get; set; }
        public string? DescriptionSv { get; set; }

        public string? DescriptionEn { get; set; }
        
        [Number(NumberType.Integer)]
        public int? FundingStartYear { get; set; }
        
        [Number(NumberType.Integer)]
        public int? FundingEndYear { get; set; }

        [Date]
        public DateTime? FundingEndDate { get; set; }

        [Nested]
        public FundingGroupPerson[]? FundingGroupPerson { get; set; }

        
        [Nested]
        public OrganizationConsortium[]? OrganizationConsortia { get; set; }

        /// <summary>
        /// OrganizationConsortia from Suomen Akatemia decisions are mapped here temporarily during db query.
        /// They are moved to 
        /// </summary>
        [Nest.Ignore]
        [System.Text.Json.Serialization.JsonIgnore]
        public OrganizationConsortium[]? OrganizationConsortia2 { get; set; }

        [Number(NumberType.Float)]
        public decimal? AmountInEur { get; set; }

        [Nested]
        public Funder? Funder { get; set; }

        [Nested]
        public FundingType? TypeOfFunding { get; set; }

        [Nested]
        public CallProgramme? CallProgramme { get; set; }

        /// <summary>
        /// Contains the "deepest" call programme parent, also known as FrameworkProgramme
        /// </summary>
        [Nested]
        public FrameworkProgramme? FrameworkProgramme { get; set; }

        /// <summary>
        /// "Temporary" property for getting parent of decision's CallProgramme.
        /// </summary>
        [Nest.Ignore]
        [System.Text.Json.Serialization.JsonIgnore]
        public FrameworkProgramme? CallProgrammeParent1 { get; set; }

        /// <summary>
        /// "Temporary" property for getting parent's parent of decision's CallProgramme.
        /// </summary>
        [Nest.Ignore]
        [System.Text.Json.Serialization.JsonIgnore]
        public FrameworkProgramme? CallProgrammeParent2 { get; set; }

        /// <summary>
        /// "Temporary" property for getting parent's parent's parent of decision's CallProgramme.
        /// </summary>
        [Nest.Ignore]
        [System.Text.Json.Serialization.JsonIgnore]
        public FrameworkProgramme? CallProgrammeParent3 { get; set; }

        /// <summary>
        /// "Temporary" property for getting parent's parent's parent's parent of decision's CallProgramme.
        /// </summary>
        [Nest.Ignore]
        [System.Text.Json.Serialization.JsonIgnore]
        public FrameworkProgramme? CallProgrammeParent4 { get; set; }

        /// <summary>
        /// "Temporary" property for getting parent's parent's parent's parent's parent of decision's CallProgramme.
        /// </summary>
        [Nest.Ignore]
        [System.Text.Json.Serialization.JsonIgnore]
        public FrameworkProgramme? CallProgrammeParent5 { get; set; }

        /// <summary>
        /// "Temporary" property for getting parent's parent's parent's parent's parent's parent of decision's CallProgramme.
        /// </summary>
        [Nest.Ignore]
        [System.Text.Json.Serialization.JsonIgnore]
        public FrameworkProgramme? CallProgrammeParent6 { get; set; }

        public string? FunderProjectNumber { get; set; }
        
        [Nested]
        public FieldOfScience[]? FieldsOfScience { get; set; }

        public string[]? Keywords { get; set; }
        public string[]? IdentifiedTopics { get; set; }

        [Nested]
        public Topic? Topic { get; set; }

        //public bool HasInternationalCollaboration { get; set; }

        //public bool HasBusinessCollaboration { get; set; }




        //[Nested]
        //public Date? ApprovalDate { get; set; }

        //[Nested]
        //public ContactPerson? ContactPerson { get; set; }

        //[Nested]
        //public Geo? Geo { get; set; }

        //[Nested]
        //public CallProgramme[]? CallProgrammes { get; set; }


        ////[Nested]
        ////public Publication[]? Publications { get; set; }

        ////[Nested]
        ////public Contribution[]? Contributions { get; set; }

        //[Nested]
        //public Infrastructure[]? Infrastructures { get; set; }


    }
}
