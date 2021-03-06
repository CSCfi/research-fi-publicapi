namespace Api
{
    public static class ApiPolicies
    {
        public const string PublicationSearch = nameof(PublicationSearch);

        public static class FundingCall
        {
            public const string Search = "FundingCallSearch";
            public const string Add = "FundingCallAdd";
        }

        public static class FundingDecision
        {
            public const string Search = "FundingDecisionSearch";
        }

        public static class Infrastructure
        {
            public const string Search = "InfrastructureSearch";
        }

        public static class Organization
        {
            public const string Search = "OrganizationSearch";
        };

        public static class ResearchDataset
        {
            public const string Search = "ResearchDatasetSearch";
        };

        /// <summary>
        /// Maps policies with their required roles.
        /// </summary>
        public static readonly Dictionary<string, string> PolicyRoleMap = new()
        {
            [FundingCall.Search] = "fundingcallreadclient",
            [FundingCall.Add] = "fundingcallwriteclient",
            [FundingDecision.Search] = "fundingdecisionreadclient",
            [Infrastructure.Search] = "infrastructurereadclient",
            [Organization.Search] = "organizationreadclient",
            [ResearchDataset.Search] = "researchdatasetreadclient"
        };
    }
}
