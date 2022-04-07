using System;
using System.Collections.Generic;

namespace Api.Models.Entities
{
    public partial class News
    {
        public int Id { get; set; }
        public string? DataJson { get; set; }
    }
}
