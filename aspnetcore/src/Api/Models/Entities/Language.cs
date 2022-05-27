using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class Language
    {
        public string Id { get; set; } = null!;
        public string? DataJson { get; set; }
    }
}
