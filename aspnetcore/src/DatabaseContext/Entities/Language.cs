using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities
{
    public partial class Language
    {
        public string Id { get; set; } = null!;
        public string? DataJson { get; set; }
    }
}
