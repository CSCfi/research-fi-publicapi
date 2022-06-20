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
        
        [Number(NumberType.Float)]
        public decimal? AmountInEur { get; set; }

        [Nested]
        public Funder? Funder { get; set; }

        [Nested]
        public FundingType? TypeOfFunding { get; set; }

        [Nested]
        public CallProgramme? CallProgramme { get; set; }

        public string? FunderProjectNumber { get; set; }
        
        [Nested]
        public FieldOfScience[]? FieldsOfScience { get; set; }

        public string[]? Keywords { get; set; }
        public string[]? IdentifiedTopics { get; set; }

        [Nested]
        public Topic? Topic { get; set; }
 
        [Nested]
        public FrameworkProgramme? FrameworkProgramme { get; set; }
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
