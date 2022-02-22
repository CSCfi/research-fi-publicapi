namespace Api
{
    public static class ApiPolicies
    {
        public const string FundingDecisionSearch = nameof(FundingDecisionSearch);
        public const string PublicationSearch = nameof(PublicationSearch);

        public static class FundingCall
        {
            public const string Search = "FundingCallSearch";
            public const string Add = "FundingCallAdd";
        }

    }
}
