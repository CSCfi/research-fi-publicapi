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

        /// <summary>
        /// Maps policies with their required roles.
        /// </summary>
        public static readonly Dictionary<string, string> PolicyRoleMap = new()
        {
            [FundingCall.Search] = "fundingcallreadclient",
            [FundingCall.Add] = "fundingcallwriteclient",
            [FundingDecision.Search] = "fundingdecisionreadclient"
        };
    }
}
