using System;
using System.Collections.Generic;

namespace CSC.PublicApi.Interface.Models.FundersAPI.ImportDb.Entities;

public partial class ImpPublicApiFundersGrantedFunding
{
    public int Id { get; set; }

    public string ClientId { get; set; } = null!;

    public string OrganizationId { get; set; } = null!;

    public string FunderProjectNumber { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public virtual ICollection<ImpPublicApiFundersGrantedFundingPublication> ImpPublicApiFundersGrantedFundingPublications { get; set; } = new List<ImpPublicApiFundersGrantedFundingPublication>();
}
