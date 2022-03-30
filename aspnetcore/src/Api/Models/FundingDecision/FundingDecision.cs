using Nest;

namespace Api.Models.FundingDecision
{
    public class FundingDecision
    {
        [Number(NumberType.Float)]
        public decimal? AmountInEur { get; set; }

        [Nested]
        public FunderOrganization? Funder { get; set; }

        public int Id { get; set; }

        public string? FunderProjectNumber { get; set; }

        public string? Acronym { get; set; }

        public string? NameUnd { get; set; }

        public string? NameFi { get; set; }

        public string? NameSv { get; set; }

        public string? NameEn { get; set; }

        public string? DescriptionFi { get; set; }

        public string? DescriptionEn { get; set; }

        public bool HasInternationalCollaboration { get; set; }

        public bool HasBusinessCollaboration { get; set; }

        [Nested]
        public FundingType? Type { get; set; }

        [Nested]
        public CallProgramme? CallProgramme { get; set; }

        [Nested]
        public Topic? Topic { get; set; }

        [Nested]
        public Date? ApprovalDate { get; set; }

        [Nested]
        public Date? FundingStartDate { get; set; }

        [Nested]
        public Date? FundingEndDate { get; set; }

        [Nested]
        public ContactPerson? ContactPerson { get; set; }

        [Nested]
        public Geo? Geo { get; set; }

        [Nested]
        public CallProgramme[]? CallProgrammes { get; set; }

        [Nested]
        public CallProgramme? FrameworkProgramme { get; set; }

        // TODO: keywords?

        [Nested]
        public FieldOfScience[]? FieldsOfScience { get; set; }

        //[Nested]
        //public Publication[]? Publications { get; set; }

        //[Nested]
        //public Contribution[]? Contributions { get; set; }

        [Nested]
        public Infrastructure[]? Infrastructures { get; set; }

        [Nested]
        public OrganizationConsortium[]? OrganizationConsortiums { get; set; }

        // TODO: funding group person?


    }

    public class Contribution
    {
        public DateTime? Created { get; set; }
        public Publication? Publication { get; set; }
    }
}
