using Nest;
using System.Text.Json.Serialization;

namespace Api.Models.FundingCall
{
    public class FundingCall
    {
        /// <summary>
        /// Haun nimi
        /// </summary>
        [Text(Name = "nameFi")]
        public string? NameFi { get; set; }

        /// <summary>
        /// Utlysningens namn
        /// </summary>
        [Text(Name = "nameSv")]
        public string? NameSv { get; set; }

        /// <summary>
        /// Funding call's name
        /// </summary>
        [Text(Name = "nameEn")]
        public string? NameEn { get; set; }

        /// <summary>
        /// Haun kuvaus
        /// </summary>
        [Text(Name = "descriptionFi")]
        public string? DescriptionFi { get; set; }

        /// <summary>
        /// Utlysningens beskrivning
        /// </summary>
        [Text(Name = "descriptionSv")]
        public string? DescriptionSv { get; set; }

        /// <summary>
        /// Description of the call
        /// </summary>
        [Text(Name = "descriptionEn")]
        public string? DescriptionEn { get; set; }

        /// <summary>
        /// Hakuohjeet
        /// </summary>
        [Text(Name = "applicationTermsFi")]
        public string? ApplicationTermsFi { get; set; }

        /// <summary>
        /// Sökanvisningar
        /// </summary>
        [Text(Name = "applicationTermsSv")]
        public string? ApplicationTermsSv { get; set; }

        /// <summary>
        /// Application guidelines
        /// </summary>
        [Text(Name = "applicationTermsEn")]
        public string? ApplicationTermsEn { get; set; }

        /// <summary>
        /// Jatkuva haku
        /// </summary>
        [Boolean(Name = "continuousApplicationPeriod")]
        public bool ContinuosApplication { get; set; }

        /// <summary>
        /// Haku alkaa
        /// </summary>
        [Date(Name = "callProgrammeOpenDate")]
        public DateTime? CallProgrammeOpenDate { get; set; }

        /// <summary>
        /// Haku päättyy
        /// </summary>
        [Date(Name = "callProgrammeDueDate")]
        public DateTime? CallProgrammeDueDate { get; set; }

        /// <summary>
        /// Rahoitushaun verkkosivu
        /// </summary>
        [Text(Name = "applicationURL_fi")]
        public string? ApplicationURLFi { get; set; }

        /// <summary>
        /// Utlysningens webbsida
        /// </summary>
        [Text(Name = "applicationURL_sv")]
        public string? ApplicationURLSv { get; set; }

        /// <summary>
        /// Webpage of the funding call
        /// </summary>
        [Text(Name = "applicationURL_en")]
        public string? ApplicationURLEn { get; set; }

        /// <summary>
        /// Rahoittajat
        /// </summary>
        [Nested]
        [PropertyName("foundation")]
        public Foundation[]? Foundation { get; set; }

        /// <summary>
        /// Hakualat
        /// </summary>
        [Nested]
        [PropertyName("categories")]
        public Category[]? Categories { get; set; }

        /// <summary>
        /// Yhteystiedot
        /// </summary>
        [Text(Name = "contactInformation")]
        public string? ContactInformation { get; set; }
    }
}
