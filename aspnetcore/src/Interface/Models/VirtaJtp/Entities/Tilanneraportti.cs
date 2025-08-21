using System;
using System.Collections.Generic;

namespace CSC.PublicApi.Interface.Models
{
    public partial class Tilanneraportti
    {
        public long TilanneraporttiId { get; set; }
        public string? Organisaationimi { get; set; }
        public string? OrganisaatioTunnus { get; set; }
        public int JulkaisuVuosi { get; set; }
        public int? IlmoitusVuosi { get; set; }
        public string? JulkaisunNimi { get; set; }
        public string? JulkaisuTyyppi { get; set; }
        public int? JulkaisunTila { get; set; }
        public string JulkaisunTunnus { get; set; } = null!;
        public string OrganisaationJulkaisuTunnus { get; set; } = null!;
        public string? Luontipaivamaara { get; set; }
        public string? Paivityspaivamaara { get; set; }
        public string? JufoTunnus { get; set; }
        public string? JufoLuokkaKoodi { get; set; }
        public string? AvoinSaatavuusJulkaisumaksu { get; set; }
        public int? AvoinSaatavuusJulkaisumaksuVuosi { get; set; }
        public string? JulkaisuKanavaOa { get; set; }
        public string? AvoinSaatavuusKytkin { get; set; }
        public string? LisenssiKoodi { get; set; }
        public string? MuotoKoodi { get; set; }
        public string? YleisoKoodi { get; set; }
        public string? EmojulkaisuntyyppiKoodi { get; set; }
        public string? ArtikkelityyppiKoodi { get; set; }
        public string? VertaisarvioituKytkin { get; set; }
        public string? RaporttiKytkin { get; set; }
        public string? OpinnayteKoodi { get; set; }
        public string? TaidetyyppiKoodi { get; set; }
        public string? AvsovellusTyyppiKoodi { get; set; }
        public string? RinnakkaistallennettuKytkin { get; set; }
    }
}
