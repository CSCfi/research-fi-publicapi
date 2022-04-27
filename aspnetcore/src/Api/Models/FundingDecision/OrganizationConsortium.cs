using Nest;

namespace Api.Models.FundingDecision
{
    public class OrganizationConsortium
    {
        //[Nested]
        //public Sector? Sector { get; set; }

        //public string? Id { get; set; }
        public string? NameFi { get; set; }
        public string? NameSv { get; set; }
        public string? NameEn { get; set; }
        public string? BusinessId { get; set; }
        //public string? Pic { get; set; }

       // public string? CountryCode { get; set; }
        public string? RoleInConsotrium { get; set; }

        [Number(NumberType.Float)]
        public decimal? ShareOfFundingInEur { get; set; }
        //public bool? IsFinnishOrganization { get; set; }

    }
}
