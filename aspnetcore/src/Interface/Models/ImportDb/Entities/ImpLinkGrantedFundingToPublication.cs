using System;
using System.Collections.Generic;

namespace CSC.PublicApi.Interface.Models.ImportDb.Entities;

public partial class ImpLinkGrantedFundingToPublication
{
    public int Id { get; set; }

    public string ClientId { get; set; } = null!;

    public string OrganizationId { get; set; } = null!;

    public string FunderProjectNumber { get; set; } = null!;

    public string? PublicationId { get; set; }

    public string? Doi { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }
}
