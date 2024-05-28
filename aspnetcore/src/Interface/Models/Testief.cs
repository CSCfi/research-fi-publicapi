using System;
using System.Collections.Generic;

namespace CSC.PublicApi.Interface.Models
{
    public partial class Testief
    {
        public long Id { get; set; }
        public string Organisaatiotunnus { get; set; } = null!;
        public string? Organisaationimi { get; set; }
    }
}
