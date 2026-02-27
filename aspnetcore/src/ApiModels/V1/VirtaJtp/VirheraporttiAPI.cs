namespace ResearchFi.VirtaJtp.Virheraportti
{
    /// <summary>
    /// Virheraportti
    /// </summary>
    public partial class VirheraporttiAPI
    {
        /// <summary>
        /// VirheraporttiId
        /// </summary>
        public long VirheraporttiId { get; set; }

        /// <summary>
        /// Organisaatiotunnus
        /// </summary>
        public string OrganisaatioTunnus { get; set; } = null!;

        /// <summary>
        /// Nimi
        /// </summary>
        public string? Nimi { get; set; }

        /// <summary>
        /// Kuvaus
        /// </summary>
        public string? Kuvaus { get; set; }

        /// <summary>
        /// Julkaisun org tunnus
        /// </summary>
        public string? JulkaisunOrgTunnus { get; set; }

        /// <summary>
        /// Julkaisun nimi
        /// </summary>
        public string? JulkaisunNimi { get; set; }

        /// <summary>
        /// Julkaisutyyppikoodi
        /// </summary>
        public string? Julkaisutyyppikoodi { get; set; }

        /// <summary>
        /// Tila
        /// </summary>
        public int? Tila { get; set; }

        /// <summary>
        /// TarkistusId
        /// </summary>
        public long TarkistusId { get; set; }

        /// <summary>
        /// Koodi
        /// </summary>
        public string? Koodi { get; set; }

        /// <summary>
        /// Virheviesti
        /// </summary>
        public string? Virheviesti { get; set; }

        /// <summary>
        /// Luontipäivämäärä
        /// </summary>
        public DateTime? Luontipaivamaara { get; set; }

        /// <summary>
        /// Ilmoitusvuosi
        /// </summary>
        public int? IlmoitusVuosi { get; set; }

        /// <summary>
        /// Julkaisuvuosi
        /// </summary>
        public int? JulkaisuVuosi { get; set; }
    }
}
