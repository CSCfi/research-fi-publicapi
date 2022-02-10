using Nest;

namespace Api.Models.FundingCall
{
    public class Foundation
    {
        /// <summary>
        /// Rahoittajan nimi
        /// </summary>
        [Text(Name = "nameFi")]
        public string? FoundationNameFi { get; set; }

        /// <summary>
        /// Rahoittajan nimi
        /// </summary>
        [Text(Name = "nameSv")]
        public string? FoundationNameSv { get; set; }

        /// <summary>
        /// Rahoittajan nimi
        /// </summary>
        [Text(Name = "nameEn")]
        public string? FoundationNameEn { get; set; }

        /// <summary>
        /// Y-tunnus
        /// </summary>
        [Text(Name = "businessId")]
        public string? FoundationBusinessId { get; set; }

        /// <summary>
        /// Rahoittajan verkkosivu
        /// </summary>
        [Text(Name = "foundationURL")]
        public string? FoundationUrl { get; set; }
    }
}
