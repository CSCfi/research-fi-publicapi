namespace CSC.PublicApi.DataAccess.Entities;

public partial class Dataset
{
    public string Id { get; set; } = null!;
    public string? DataJson { get; set; }
}