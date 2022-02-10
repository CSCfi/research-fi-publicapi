using Nest;

namespace Api.Models.FundingCall
{
    public class Category
    {
        /// <summary>
        /// Hakualan koodi
        /// </summary>
        [Text(Name = "codeValue")]
        public string? CategoryCodeValue { get; set; }

        /// <summary>
        /// Hakuala
        /// </summary>
        [Text(Name = "nameFi")]
        public string? CategoryNameFi { get; set; }

        /// <summary>
        /// Hakuala
        /// </summary>
        [Text(Name = "nameSv")]
        public string? CategoryNameSv { get; set; }

        /// <summary>
        /// Hakuala
        /// </summary>
        [Text(Name = "nameEn")]
        public string? CategoryNameEn { get; set; }
    }
}
