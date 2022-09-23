using Microsoft.AspNetCore.Mvc;

namespace Api.Models.FundingCall
{
    public class FundingCallSearchParameters
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }

        [FromQuery(Name = "foundationName")]
        public string? FoundationName { get; set; }

        [FromQuery(Name = "foundationBusinessId")]
        public string? FoundationBusinessId { get; set; }

        [FromQuery(Name = "categoryCode")]
        public string? CategoryCode { get; set; }

        /// <summary>
        /// Haku alkaa aikaisintaan. Päivämäärä muodossa vvvv-kk-pp
        /// </summary>
        [FromQuery(Name = "dateFrom")]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Haku päättyy viimeistään. Päivämäärä muodossa vvvv-kk-pp
        /// </summary>
        [FromQuery(Name = "dateTo")]
        public DateTime? DateTo { get; set; }
    }
}
