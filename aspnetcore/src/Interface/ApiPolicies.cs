namespace CSC.PublicApi.Interface;

public static class ApiPolicies
{
    public const string PublicationSearch = nameof(PublicationSearch);

    public static class FundingCall
    {
        public const string Read = "FundingCallRead";
        public const string Write = "FundingCallWrite";
    }

    public static class FundingDecision
    {
        public const string Read = "FundingDecisionRead";
    }

    public static class Infrastructure
    {
        public const string Read = "InfrastructureRead";
    }

    public static class Organization
    {
        public const string Read = "OrganizationRead";
    };

    public static class ResearchDataset
    {
        public const string Read = "ResearchDatasetRead";
    };

    /// <summary>
    /// Maps policies with their required roles.
    /// </summary>
    public static readonly Dictionary<string, string> PolicyRoleMap = new()
    {
        [FundingCall.Read] = "funding-call-read",
        [FundingCall.Write] = "funding-call-write",
        [FundingDecision.Read] = "funding-decision-read",
        [Infrastructure.Read] = "infrastructure-read",
        [Organization.Read] = "organization-read",
        [ResearchDataset.Read] = "research-dataset-read"
    };
}