namespace CSC.PublicApi.DataAccess.Entities;

public partial class FactInfraKeyword
{
    public int DimKeywordId { get; set; }
    public int DimServiceId { get; set; }
    public int DimServicePointId { get; set; }
    public int DimInfrastructureId { get; set; }
    public string SourceId { get; set; } = null!;
    public string? SourceDescription { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }

    public virtual DimInfrastructure DimInfrastructure { get; set; } = null!;
    public virtual DimKeyword DimKeyword { get; set; } = null!;
    public virtual DimService DimService { get; set; } = null!;
    public virtual DimServicePoint DimServicePoint { get; set; } = null!;
}