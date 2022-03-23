using Nest;

namespace Api.Models.FundingDecision
{
    public class FundingDecision
    {
        [Number(NumberType.Float)]
        public decimal? AmountInEur { get; set; }

        [Nested]
        public FunderOrganization? Funder { get; set; }

        public string? ProjectId { get; set; }

        public string? FunderProjectNumber { get; set; }

        public string? ProjectAcronym { get; set; }

        public string? ProjectNameUnd { get; set; }

        public string? ProjectNameFi { get; set; }

        public string? ProjectNameSv { get; set; }

        public string? ProjectNameEn { get; set; }

        public string? ProjectDescriptionFi { get; set; }

        public string? ProjectDescriptionEn { get; set; }

        public bool? InternationalCollaboration { get; set; }

        public bool? BusinessCollaboration { get; set; }

        [Nested]
        public Sector? FunderSector { get; set; }

        [Nested]
        public FundingType? Type { get; set; }

        [Nested]
        public CallProgramme? CallProgramme { get; set; }

        [Nested]
        public Topic? Topic { get; set; }

        [Date]
        public DateTime? ApprovalDate { get; set; }

        [Date]
        public DateTime? FundingStartDate { get; set; }

        [Date]
        public DateTime? FundingEndDate { get; set; }

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

        [Nested]
        public Publication[]? Publications { get; set; }

        [Nested]
        public Infrastructure[]? Infrastructures { get; set; }

        [Nested]
        public OrganizationConsortium[]? OrganizationConsortiums { get; set; }

        // TODO: funding group person?


    }
}
