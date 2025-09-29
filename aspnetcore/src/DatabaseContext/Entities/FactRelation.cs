﻿using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class FactRelation
{
    public int RelationTypeCode { get; set; }

    public int FromPublicationId { get; set; }

    public int FromResearchDatasetId { get; set; }

    public int FromIdentifierlessDataId { get; set; }

    public int FromInfrastructureId { get; set; }

    public int ToResearchDatasetId { get; set; }

    public int ToIdentifierlessDataId { get; set; }

    public int ToPublicationId { get; set; }

    public int ToInfrastructureId { get; set; }

    public int StartDate { get; set; }

    public int EndDate { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public string? SourceDescription { get; set; }

    public string? SourceId { get; set; }

    public bool? ValidRelation { get; set; }

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;

    public virtual DimDate EndDateNavigation { get; set; } = null!;

    public virtual DimIdentifierlessDatum FromIdentifierlessData { get; set; } = null!;

    public virtual DimInfrastructure FromInfrastructure { get; set; } = null!;

    public virtual DimPublication FromPublication { get; set; } = null!;

    public virtual DimResearchDataset FromResearchDataset { get; set; } = null!;

    public virtual DimReferencedatum RelationTypeCodeNavigation { get; set; } = null!;

    public virtual DimDate StartDateNavigation { get; set; } = null!;

    public virtual DimIdentifierlessDatum ToIdentifierlessData { get; set; } = null!;

    public virtual DimInfrastructure ToInfrastructure { get; set; } = null!;

    public virtual DimPublication ToPublication { get; set; } = null!;

    public virtual DimResearchDataset ToResearchDataset { get; set; } = null!;
}
