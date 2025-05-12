using System;
using System.Collections.Generic;

namespace CSC.PublicApi.Interface.Models.FundersAPI.ImportDb.Entities;

public partial class ImpPublicApiFundersGrantedFundingPublication
{
    public int Id { get; set; }

    public string ClientId { get; set; } = null!;

    public int FkGrantedFunding { get; set; }

    public string? PublicationId { get; set; }

    public string? Doi { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public virtual ImpPublicApiFundersGrantedFunding FkGrantedFundingNavigation { get; set; } = null!;
}
