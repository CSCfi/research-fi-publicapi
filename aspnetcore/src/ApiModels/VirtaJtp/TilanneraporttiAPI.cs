namespace ResearchFi.VirtaJtp.Tilanneraportti
{
    /// <summary>
    /// Tilanneraportti
    /// </summary>
    public partial class TilanneraporttiAPI
    {
        /// <summary>
        /// TilanneraporttiId
        /// </summary>
        public long TilanneraporttiId { get; set; }

        /// <summary>
        /// Organisaationimi
        /// </summary>
        public string? Organisaationimi { get; set; }

        /// <summary>
        /// Organisaatiotunnus
        /// </summary>
        public string? OrganisaatioTunnus { get; set; }

        /// <summary>
        /// Julkaisuvuosi
        /// </summary>
        public int JulkaisuVuosi { get; set; }

        /// <summary>
        /// Ilmoitusvuosi
        /// </summary>
        public int? IlmoitusVuosi { get; set; }

        /// <summary>
        /// Julkaisun nimi
        /// </summary>
        public string? JulkaisunNimi { get; set; }

        /// <summary>
        /// Julkaisun tyyppi
        /// </summary>
        public string? JulkaisuTyyppi { get; set; }

        /// <summary>
        /// Julkaisun tila
        /// </summary>
        public int? JulkaisunTila { get; set; }

        /// <summary>
        /// Julkaisun tunnus
        /// </summary>
        public string JulkaisunTunnus { get; set; } = null!;

        /// <summary>
        /// Organisaation julkaisutunnus
        /// </summary>
        public string OrganisaationJulkaisuTunnus { get; set; } = null!;

        /// <summary>
        /// Luontipäivämäärä
        /// </summary>
        public string? Luontipaivamaara { get; set; }

        /// <summary>
        /// Päivityspäivämäärä
        /// </summary>
        public string? Paivityspaivamaara { get; set; }

        /// <summary>
        /// Jufotunnus
        /// </summary>
        public string? JufoTunnus { get; set; }

        /// <summary>
        /// Jufoluokkakoodi
        /// </summary>
        public string? JufoLuokkaKoodi { get; set; }

        /// <summary>
        /// Avoin saatavuus julkaisumaksu
        /// </summary>
        public string? AvoinSaatavuusJulkaisumaksu { get; set; }

        /// <summary>
        /// Avoin saatavuus julkaisumaksu vuosi
        /// </summary>
        public int? AvoinSaatavuusJulkaisumaksuVuosi { get; set; }

        /// <summary>
        /// Julkaisukanava Oa
        /// </summary>
        public string? JulkaisuKanavaOa { get; set; }

        /// <summary>
        /// Avoin saatavuus kytkin
        /// </summary>
        public string? AvoinSaatavuusKytkin { get; set; }

        /// <summary>
        /// Lisenssikoodi
        /// </summary>
        public string? LisenssiKoodi { get; set; }

        /// <summary>
        /// Muotokoodi
        /// </summary>
        public string? MuotoKoodi { get; set; }

        /// <summary>
        /// Yleisökoodi
        /// </summary>
        public string? YleisoKoodi { get; set; }

        /// <summary>
        /// Emojulkaisun tyyppikoodi
        /// </summary>
        public string? EmojulkaisuntyyppiKoodi { get; set; }

        /// <summary>
        /// Artikkelityyppikoodi
        /// </summary>
        public string? ArtikkelityyppiKoodi { get; set; }

        /// <summary>
        /// Vertaisarvioitukytkin
        /// </summary>
        public string? VertaisarvioituKytkin { get; set; }

        /// <summary>
        /// Raporttikytkin
        /// </summary>
        public string? RaporttiKytkin { get; set; }

        /// <summary>
        /// Opinnäytekoodi
        /// </summary>
        public string? OpinnayteKoodi { get; set; }

        /// <summary>
        /// Taidetyyppikoodi
        /// </summary>
        public string? TaidetyyppiKoodi { get; set; }

        /// <summary>
        /// Avsovellustyyppikoodi
        /// </summary>
        public string? AvsovellusTyyppiKoodi { get; set; }

        /// <summary>
        /// Rinnakkaistallennettu kytkin
        /// </summary>
        public string? RinnakkaistallennettuKytkin { get; set; }
    }
}
