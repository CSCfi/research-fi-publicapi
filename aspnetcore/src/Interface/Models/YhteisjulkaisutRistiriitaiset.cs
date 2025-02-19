using System;
using System.Collections.Generic;

namespace CSC.PublicApi.Interface.Models
{
    public partial class YhteisjulkaisutRistiriitaiset
    {
        public long RrId { get; set; }
        public string YhteisjulkaisunTunnus { get; set; } = null!;
        public string JulkaisunTunnus { get; set; } = null!;
        public string? Organisaationimi { get; set; }
        public string OrganisaatioTunnus { get; set; } = null!;
        public int? JulkaisuVuosi { get; set; }
        public int? IlmoitusVuosi { get; set; }
        public string JulkaisunNimi { get; set; } = null!;
        public string? Julkaisutyyppi { get; set; }
        public string? JulkaisunOrgTunnus { get; set; }
        public string? LiittyvaJulkaisunOrgTunnus { get; set; }
        public string? Luontipaivamaara { get; set; }
        public string? Koodi { get; set; }
        public string? Kuvaus { get; set; }
        public int? Tila { get; set; }
    }
}
