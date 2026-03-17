using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimDescriptiveItem
{
    public int Id { get; set; }

    public int DimStartDate { get; set; }

    public int? DimEndDate { get; set; }

    public string DescriptiveItem { get; set; } = null!;

    public string DescriptiveItemType { get; set; } = null!;

    public string DescriptiveItemLanguage { get; set; } = null!;

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int DimResearchProjectId { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public int DimPublicationId { get; set; }

    public int DimResearchDatasetId { get; set; }

    public int DimInfrastructureId { get; set; }

    public int DimServiceId { get; set; }

    public int DimResearchDataCatalogId { get; set; }

    public virtual DimDate? DimEndDateNavigation { get; set; }

    public virtual DimInfrastructure DimInfrastructure { get; set; } = null!;

    public virtual DimPublication DimPublication { get; set; } = null!;

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;

    public virtual DimResearchDataCatalog DimResearchDataCatalog { get; set; } = null!;

    public virtual DimResearchDataset DimResearchDataset { get; set; } = null!;

    public virtual DimResearchProject DimResearchProject { get; set; } = null!;

    public virtual DimService DimService { get; set; } = null!;

    public virtual DimDate DimStartDateNavigation { get; set; } = null!;
}
