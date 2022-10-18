using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities
{
    public partial class FundingCall
    {
        public int Id { get; set; }
        public string? DataJson { get; set; }
    }
}
