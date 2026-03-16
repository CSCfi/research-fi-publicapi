using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class PidM
{
    public int Id { get; set; }

    public int? DimInfrastructureId { get; set; }

    public int? DimServiceId { get; set; }

    public string ActionType { get; set; } = null!;

    public DateTime RowCreated { get; set; }

    public DateTime? Completed { get; set; }
}
