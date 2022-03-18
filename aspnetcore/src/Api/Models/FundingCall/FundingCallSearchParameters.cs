namespace Api.Models.FundingCall
{
    public class FundingCallSearchParameters
    {
        public string? Name { get; set; }
        public string? FoundationName { get; set; }
        public string? FoundationBusinessId { get; set; }
        public string? CategoryCode { get; set; }

        /// <summary>
        /// Haku alkaa aikaisintaan
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Haku päättyy viimeistään
        /// </summary>
        public DateTime? DateTo { get; set; }

    }
}
