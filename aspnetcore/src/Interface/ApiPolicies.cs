namespace CSC.PublicApi.Interface;

public static class ApiPolicies
{
    public static class Publication
    {
        public const string Read = "PublicationRead";
    };

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

    public static class Funder
    {
        public const string Read = "FunderRead";
        public const string Write = "FunderWrite";
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
        [ResearchDataset.Read] = "research-dataset-read",
        [Publication.Read] = "publication-read",
        [Funder.Read] = "funder-read",
        [Funder.Write] = "funder-write"
    };
}