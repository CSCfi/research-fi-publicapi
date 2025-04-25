using System;
using System.Collections.Generic;

namespace CSC.PublicApi.Interface.Models.ImportDb.Entities;

public partial class ImpLinkPublicationToGrantedFunding
{
    public long Id { get; set; }

    public string? PublicationId { get; set; }

    public string OrganizationId { get; set; } = null!;

    public string FunderProjectNumber { get; set; } = null!;

    public DateTime Created { get; set; }

    public string? ClientId { get; set; }

    public DateTime? Modified { get; set; }

    public string? Doi { get; set; }
}
