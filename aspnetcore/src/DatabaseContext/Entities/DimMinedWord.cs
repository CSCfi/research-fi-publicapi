namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimMinedWord
{
    public DimMinedWord()
    {
        BrWordsDefineAClusters = new HashSet<BrWordsDefineACluster>();
    }

    public int Id { get; set; }
    public string Word { get; set; } = null!;
    public string? SourceDescription { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public string SourceId { get; set; } = null!;

    public virtual ICollection<BrWordsDefineACluster> BrWordsDefineAClusters { get; set; }
}