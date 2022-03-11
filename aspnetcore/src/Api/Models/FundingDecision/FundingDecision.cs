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

        [Text(Name = "projectDescriptionFi")]
        public string? DescriptionFi { get; set; }

        [Text(Name = "projectDescriptionSv")]
        public string? DescriptionSv { get; set; }

        [Text(Name = "projectDescriptionEn")]
        public string? DescriptionEn { get; set; }


    }
}
