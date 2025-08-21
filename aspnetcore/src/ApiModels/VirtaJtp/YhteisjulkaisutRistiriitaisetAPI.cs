namespace ResearchFi.VirtaJtp.YhteisjulkaisutRistiriitaiset
{
    /// <summary>
    /// YhteisjulkaisutRistiriitaiset
    /// </summary>
    public partial class YhteisjulkaisutRistiriitaisetApi
    {
        /// <summary>
        /// RrId
        /// </summary>
        public long RrId { get; set; }

        /// <summary>
        /// Yhteisjulkaisun tunnus
        /// </summary>
        public string YhteisjulkaisunTunnus { get; set; } = null!;

        /// <summary>
        /// Julkaisun tunnus
        /// </summary>
        public string JulkaisunTunnus { get; set; } = null!;

        /// <summary>
        /// Organisaation nimi
        /// </summary>
        public string? Organisaationimi { get; set; }

        /// <summary>
        /// Organisaatiotunnus
        /// </summary>
        public string OrganisaatioTunnus { get; set; } = null!;

        /// <summary>
        /// Julkaisuvuosi
        /// </summary>
        public int? JulkaisuVuosi { get; set; }

        /// <summary>
        /// Ilmoitusvuosi
        /// </summary>
        public int? IlmoitusVuosi { get; set; }

        /// <summary>
        /// Julkaisun nimi
        /// </summary>
        public string JulkaisunNimi { get; set; } = null!;

        /// <summary>
        /// Ristiriitainen tieto
        /// </summary>
        public string? RistiriitainenTieto { get; set; } // In API "Julkaisutyyppi" in the DB is mapped to "RistiriitainenTieto"

        /// <summary>
        /// Julkaisun org tunnus
        /// </summary>
        public string? JulkaisunOrgTunnus { get; set; }

        /// <summary>
        /// Liittyvä julkaisun org tunnus
        /// </summary>
        public string? LiittyvaJulkaisunOrgTunnus { get; set; }

        /// <summary>
        /// Luontipäivämäärä
        /// </summary>
        public string? Luontipaivamaara { get; set; }

        /// <summary>
        /// Koodi
        /// </summary>
        public string? Koodi { get; set; }

        /// <summary>
        /// Kuvaus
        /// </summary>
        public string? Kuvaus { get; set; }

        /// <summary>
        /// Tila
        /// </summary>
        public int? Tila { get; set; }
    }
}
