namespace ResearchFi.VirtaJtp.Duplikaatit
{
    /// <summary>
    /// Duplikaatit
    /// </summary>
    public partial class DuplikaatitAPI
    {
        /// <summary>
        /// DuplikaattiId
        /// </summary>
        public long DuplikaattiId { get; set; }
        /// <summary>
        /// Organisaatiotunnus
        /// </summary>
        public string Organisaatiotunnus { get; set; } = null!;
        /// <summary>
        /// Organisaationimi
        /// </summary>
        public string? Organisaationimi { get; set; }
        /// <summary>
        /// Julkaisun tunnus
        /// </summary>
        public string Julkaisuntunnus { get; set; } = null!;
        /// <summary>
        /// Kuvaus
        /// </summary>
        public string? Kuvaus { get; set; }
        /// <summary>
        /// Julkaisun org tunnus
        /// </summary>
        public string Julkaisunorgtunnus { get; set; } = null!;
        /// <summary>
        /// Duplikaattijulkaisun org tunnus
        /// </summary>
        public string Duplikaattijulkaisunorgtunnus { get; set; } = null!;
        /// <summary>
        /// Julkaisun nimi
        /// </summary>
        public string? Julkaisunnimi { get; set; }
        /// <summary>
        /// Duplikaattijulkaisun nimi
        /// </summary>
        public string? Duplikaattijulkaisunnimi { get; set; }
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
        /// Luontipäivämäärä
        /// </summary>
        public DateTime? Luontipaivamaara { get; set; }
        /// <summary>
        /// Ilmoitusvuosi
        /// </summary>
        public int? Ilmoitusvuosi { get; set; }
        /// <summary>
        /// Julkaisuvuosi
        /// </summary>
        public int? Julkaisuvuosi { get; set; }
    }
}
