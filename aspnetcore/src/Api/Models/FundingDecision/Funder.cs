using Nest;

namespace Api.Models.FundingDecision
{
    public class Funder
    {
        public string? NameFi { get; set; }
        public string? NameSv { get; set; }
        public string? NameEn { get; set; }
        [Nested]
        public Id[]? Ids { get; set; }
    }
}
