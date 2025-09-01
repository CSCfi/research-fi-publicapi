using System;
using System.Collections.Generic;

namespace CSC.PublicApi.DatabaseContext.Entities;

public partial class DimResearchProject
{
    public int Id { get; set; }

    /// <summary>
    /// Hanke - vastuuorganisaatio
    /// </summary>
    public int ResponsibleOrganization { get; set; }

    /// <summary>
    /// Hanke - alkamispäivämäärä
    /// </summary>
    public int? StartDate { get; set; }

    /// <summary>
    /// Hanke - päättymispäivämäärä
    /// </summary>
    public int? EndDate { get; set; }

    public string SourceId { get; set; } = null!;

    public string? SourceDescription { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public int DimRegisteredDataSourceId { get; set; }

    public long ResponsiblePerson { get; set; }

    public virtual ICollection<DimDescriptiveItem> DimDescriptiveItems { get; set; } = new List<DimDescriptiveItem>();

    public virtual DimRegisteredDataSource DimRegisteredDataSource { get; set; } = null!;

    public virtual DimDate? EndDateNavigation { get; set; }

    public virtual ICollection<FactKeyword> FactKeywords { get; set; } = new List<FactKeyword>();

    public virtual DimOrganization ResponsibleOrganizationNavigation { get; set; } = null!;

    public virtual DimName ResponsiblePersonNavigation { get; set; } = null!;

    public virtual DimDate? StartDateNavigation { get; set; }
}
