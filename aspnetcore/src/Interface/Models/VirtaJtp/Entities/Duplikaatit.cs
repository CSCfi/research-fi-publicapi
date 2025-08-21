using System;
using System.Collections.Generic;

namespace CSC.PublicApi.Interface.Models
{
    public partial class Duplikaatit
    {
        public long DuplikaattiId { get; set; }
        public string Organisaatiotunnus { get; set; } = null!;
        public string? Organisaationimi { get; set; }
        public string Julkaisuntunnus { get; set; } = null!;
        public string? Kuvaus { get; set; }
        public string Julkaisunorgtunnus { get; set; } = null!;
        public string Duplikaattijulkaisunorgtunnus { get; set; } = null!;
        public string? Julkaisunnimi { get; set; }
        public string? Duplikaattijulkaisunnimi { get; set; }
        public string? Julkaisutyyppikoodi { get; set; }
        public int? Tila { get; set; }
        public long TarkistusId { get; set; }
        public DateTime? Luontipaivamaara { get; set; }
        public int? Ilmoitusvuosi { get; set; }
        public int? Julkaisuvuosi { get; set; }
    }
}
