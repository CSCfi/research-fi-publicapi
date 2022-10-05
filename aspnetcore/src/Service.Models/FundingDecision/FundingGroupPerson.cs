﻿using Nest;

namespace CSC.PublicApi.Service.Models.FundingDecision;

public class FundingGroupPerson
{
    public string? FirstNames { get; set; }
    public string? LastName { get; set; }

    [Keyword]
    public string? OrcId { get; set; }
    public string? RoleInFundingGroup { get; set; }
}