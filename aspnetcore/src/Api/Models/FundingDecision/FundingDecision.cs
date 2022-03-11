using Nest;

namespace Api.Models.FundingDecision
{
    public class FundingDecision
    {
        [Text(Name="projectNameFi")]
        public string? NameFi { get; set; }

        [Text(Name = "projectNameSv")]
        public string? NameSv { get; set; }

        [Text(Name = "projectNameEn")]
        public string? NameEn { get; set; }

    }
}
