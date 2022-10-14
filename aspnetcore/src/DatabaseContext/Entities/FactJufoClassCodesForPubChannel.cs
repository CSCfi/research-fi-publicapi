namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class FactJufoClassCodesForPubChannel
{
    public int DimPublicationChannelId { get; set; }
    public int DimReferencedataId { get; set; }
    public int Year { get; set; }

    public virtual DimPublicationChannel DimPublicationChannel { get; set; } = null!;
    public virtual DimReferencedatum DimReferencedata { get; set; } = null!;
}