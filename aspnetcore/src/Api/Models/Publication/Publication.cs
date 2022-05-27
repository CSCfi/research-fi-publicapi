
namespace Api.Models.Publication
{
    public class Publication
    {
        /// <summary>
        /// Julkaisun nimi
        /// </summary>
        public string? publicationName { get; set; }

        /// <summary>
        /// Julkaisuvuosi
        /// </summary>
        public int publicationYear { get; set; }

        /// <summary>
        /// Tekijät
        /// </summary>
        public string? authorsText { get; set; }

        public Author[]? author { get; set; }

        /// <summary>
        /// Organisaatiotunnus
        /// </summary>
        public string? publicationOrgId { get; set; }

        /// <summary>
        /// Julkaisun muoto
        /// </summary>
        //public string? publicationTypeCode2 { get; set; }

        /// <summary>
        /// Emojulkaisu
        /// </summary>
        public ParentPublicationType[]? parentPublicationType { get; set; }

        /// <summary>
        /// Vertaisarvioitu
        /// </summary>
        public PeerReviewed[]? peerReviewed { get; set; }

        /// <summary>
        /// Yleisö
        /// </summary>
        //public string? targetAudienceCode { get; set; }

        /// <summary>
        /// OKM:n julkaisutyyppiluokitus
        /// </summary>
        public string? publicationTypeCode { get; set; }

        /// <summary>
        /// Lehti
        /// </summary>
        public string? journalName { get; set; }

        /// <summary>
        /// Konferenssi
        /// </summary>
        //public string? conferenceName { get; set; }

        /// <summary>
        /// ISSN
        /// </summary>
        public string? issn { get; set; }

        /// <summary>
        /// ISSN2
        /// </summary>
        public string? issn2 { get; set; }

        /// <summary>
        /// Volyymi
        /// </summary>
        public string? volume { get; set; }

        /// <summary>
        /// Numero
        /// </summary>
        public string? issueNumber { get; set; }

        /// <summary>
        /// Sivut
        /// </summary>
        //public string? pageNumberText { get; set; }

        /// <summary>
        /// Artikkelinumero
        /// </summary>
        //public string? articleNumberText { get; set; }

        /// <summary>
        /// Emojulkaisun nimi
        /// </summary>
        //public string? parentPublicationName { get; set; }

        /// <summary>
        /// Emojulkaisun toimittajat
        /// </summary>
        //public string? parentPublicationPublisher { get; set; }

        /// <summary>
        /// ISBN
        /// </summary>
        public string? isbn { get; set; }

        /// <summary>
        /// ISBN2
        /// </summary>
        public string? isbn2 { get; set; }

        /// <summary>
        /// Kustantaja
        /// </summary>
        public string? publisherName { get; set; }

        /// <summary>
        /// Kustannuspaikka
        /// </summary>
        //public string? publisherLocation { get; set; }

        /// <summary>
        /// Julkaisufoorumi
        /// </summary>
        //public string? jufoCode { get; set; }

        /// <summary>
        /// Julkaisufoorumitaso
        /// </summary>
        //public string? jufoClassCode { get; set; }

        /// <summary>
        /// Linkit
        /// </summary>
        public string? doi { get; set; }

        /// <summary>
        /// Pysyvä osoite
        /// </summary>
        public string? doiHandle { get; set; }

        /// <summary>
        /// Rinnakkaistallenne
        /// </summary>
        public SelfArchivedData[]? selfArchivedData { get; set; }

        /// <summary>
        /// Tieteenalat
        /// </summary>
        public FieldOfScience[]? fieldsOfScience { get; set; }

        /// <summary>
        /// OKM:n ohjauksen ala
        /// </summary>
        public FieldOfEducation[]? fields_of_education { get; set; }

        /// <summary>
        /// Avainsanat
        /// </summary>
        //public string? keywords { get; set; }

        /// <summary>
        /// Avoin saatavuus
        /// </summary>
        public int? openAccessCode { get; set; }

        /// <summary>
        /// Julkaisun kansainvälisyys
        /// </summary>
        //public bool? internationalPublication { get; set; }

        /// <summary>
        /// Julkaisumaa
        /// </summary>
        public string? publicationCountryCode { get; set; }

        /// <summary>
        /// Kieli
        /// </summary>
        //public string? languageCode { get; set; }

        /// <summary>
        /// Kansainvälinen yhteisjulkaisu
        /// </summary>
        public bool? internationalCollaboration { get; set; }

        /// <summary>
        /// Yhteisjulkaisu yrityksen kanssa
        /// </summary>
        public bool? businessCollaboration { get; set; }

        /*
         * Avoin saatavuus julkaisumaksu 
         * Avoin saatavuus julkaisumaksun vuosi 
         * Artikkelin tyyppi
         * Avoin saatavuus kytkin 
         * Ilmoitusvuosi 
         * Julkaisun tila
         * Kuvaus 
         * Lisenssikoodi 
         * Opinnäyte 
         * Preprint 
         * Raportti
         * Rinnakkaistallenne 
         * Taidealan julkaisun alkupvm 
         * Taidealan julkaisun loppupvm 
         * Taidealan julkaisun toistojen lkm 
         * Taiteenalakoodi
         * Tekijöiden lukumäärä 
         * Luontipvm
         * Muokkauspvm
         */

    }
}