using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimContactInformation
{
    public int Id { get; set; }

    public int DimInfrastructureId { get; set; }

    public int DimServiceId { get; set; }

    public string ContactName { get; set; } = null!;

    public string? ContactInformationContent { get; set; }

    public string? ContactInformationType { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public virtual DimInfrastructure DimInfrastructure { get; set; } = null!;

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;

    public virtual DimService DimService { get; set; } = null!;
}
