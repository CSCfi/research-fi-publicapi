using System;
using System.Collections.Generic;

namespace CSC.PublicApi.Interface.Models
{
    public partial class Virheraportti
    {
        public long VirheraporttiId { get; set; }
        public string OrganisaatioTunnus { get; set; } = null!;
        public string? Nimi { get; set; }
        public string? Kuvaus { get; set; }
        public string? JulkaisunOrgTunnus { get; set; }
        public string? JulkaisunNimi { get; set; }
        public string? Julkaisutyyppikoodi { get; set; }
        public int? Tila { get; set; }
        public long TarkistusId { get; set; }
        public string? Koodi { get; set; }
        public string? Virheviesti { get; set; }
        public DateTime? Luontipaivamaara { get; set; }
        public int? IlmoitusVuosi { get; set; }
        public int? JulkaisuVuosi { get; set; }
    }
}
