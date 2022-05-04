namespace Api.Models.FundingDecision
{
    public class Funder
    {
        public string? NameFi { get; set; }
        public string? NameSv { get; set; }
        public string? NameEn { get; set; }

        public Id[]? Ids { get; set; }
        //public int? Id { get; set; }
        //public string? LocalOrganizationUnitId { get; set; }
        //public string? LocalOrganizationSector { get; set; }
        //public string? OrganizationType { get; set; }
        //public string BusinessIds { get; set; }

        //[Nested]
        //public Sector? FunderSector { get; set; }


    }
}
