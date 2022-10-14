namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class BrKeywordDimKeyword
{
    public int DimKeywordId { get; set; }
    public int DimKeywordId2 { get; set; }
    public int DimReferencedataIdRelationshipTypeCode { get; set; }

    public virtual DimKeyword DimKeyword { get; set; } = null!;
    public virtual DimKeyword DimKeywordId2Navigation { get; set; } = null!;
    public virtual DimReferencedatum DimReferencedataIdRelationshipTypeCodeNavigation { get; set; } = null!;
}