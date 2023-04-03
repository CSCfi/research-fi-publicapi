using ResearchFi.CodeList;

namespace ResearchFi.ResearchDataset;

/// <summary>
/// Tutkimusaineisto
/// </summary>
public class ResearchDataset
{
    /// <summary>
    /// Tutkimusaineiston paikallinen tunniste
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Tutkimusaineiston nimi
    /// </summary>
    public string? NameFi { get; set; }

    /// <summary>
    /// Namnet på forskningsmaterialet
    /// </summary>
    public string? NameSv { get; set; }

    /// <summary>
    /// Name of the research dataset
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// Tutkimusaineiston kuvaus
    /// </summary>
    public string? DescriptionFi { get; set; }

    /// <summary>
    /// Beskrivning av forskningsmaterialet
    /// </summary>
    public string? DescriptionSv { get; set; }

    /// <summary>
    /// Description of the research dataset
    /// </summary>
    public string? DescriptionEn { get; set; }

    /// <summary>
    /// Julkaisuvuosi
    /// </summary>
    public DateTime? Created { get; set; }

    /// <summary>
    /// Tekijät
    /// </summary>
    public List<Contributor>? Contributors { get; set; }

    /// <summary>
    /// Tieteenalat
    /// 
    /// http://uri.suomi.fi/codelist/research/Tieteenala2010
    /// </summary>
    public List<FieldOfScience>? FieldsOfScience { get; set; }

    /// <summary>
    /// Kielet
    /// 
    /// http://lexvo.org/id/iso639-3
    /// </summary>
    public List<LexvoLanguage>? Languages { get; set; }

    /// <summary>
    /// Saatavuus
    /// 
    /// http://uri.suomi.fi/codelist/fairdata/access_type
    /// </summary>
    public AccessType? AccessType { get; set; }

    /// <summary>
    /// Lisenssi
    /// 
    /// http://uri.suomi.fi/codelist/fairdata/license
    /// </summary>
    public License? License { get; set; }

    /// <summary>
    /// Avainsanat
    /// </summary>
    public List<Keyword>? Keywords { get; set; }

    /// <summary>
    /// Liittyvät tutkimusaineistot
    /// </summary>
    public List<DatasetRelation>? DatasetRelations { get; set; }

    /// <summary>
    /// Versiot
    /// </summary>
    public List<Version>? VersionSet { get; set; }

    /// <summary>
    /// Pysyvät tunnisteet
    /// </summary>
    public List<PersistentIdentifier>? PersistentIdentifiers { get; set; }

    /// <summary>
    /// Fairdata URL
    /// </summary>
    public string? FairDataUrl { get; set; }

    /// <summary>
    /// Tutkimusaineistotiedon lähde
    /// </summary>
    public ResearchDataCatalog? ResearchDataCatalog { get; set; }

    /// <summary>
    /// Uusin versio
    /// </summary>
    public bool IsLatestVersion { get; set; }

}