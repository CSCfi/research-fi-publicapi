using Api.Configuration;
using Api.Models.FundingDecision;
using Nest;

namespace Api.Services.ElasticSearchQueryGenerators
{
    public class FundingDecisionQueryGenerator : QueryGeneratorBase<FundingDecisionSearchParameters, FundingDecision>
    {
        public FundingDecisionQueryGenerator(IndexNameSettings configuration) : base(configuration)
        {
        }

        protected override Func<SearchDescriptor<FundingDecision>, ISearchRequest> GenerateQueryForIndex(FundingDecisionSearchParameters searchParameters, string indexName)
        {
            var subQueries = new List<Func<QueryContainerDescriptor<FundingDecision>, QueryContainer>>();
            var filters = new List<Func<QueryContainerDescriptor<FundingDecision>, QueryContainer>>();

            // When searching with Name, search from Fi,Sv,En names.
            if (!string.IsNullOrWhiteSpace(searchParameters.Name))
            {
                subQueries.Add(t => t.MultiMatch(query => query
                    .Type(TextQueryType.PhrasePrefix)
                    .Fields("nameFi, nameSv, nameEn")
                    .Query(searchParameters.Name)));
            }

            // When searching with Description, search from Fi,Sv,En descriptions.
            if (!string.IsNullOrWhiteSpace(searchParameters.Description))
            {
                subQueries.Add(t => t.MultiMatch(query => query
                    .Type(TextQueryType.PhrasePrefix)
                    .Fields("descriptionFi, descriptionSv, descriptionEn")
                    .Query(searchParameters.Description)));
            }

            // When searching with FoundationName, search from Fi,Sv,En names.
            if (!string.IsNullOrWhiteSpace(searchParameters.FunderName))
            {
                subQueries.Add(t => t.Nested(query => query
                    .Path("funder")
                    .Query(q => q.MultiMatch(query => query
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields("funder.nameFi, funder.nameSv, funder.nameEn")
                        .Query(searchParameters.FunderName)
                    ))));
            }

            // Searching with funder id requires exact match.
            if (!string.IsNullOrWhiteSpace(searchParameters.FunderId))
            {
                subQueries.Add(t => t.Nested(query => query
                    .Path("funder.ids")
                    .Query(q => q.Term(term => term
                        .Field("funder.ids.content")
                        .Value(searchParameters.FunderId)
                    ))));
            }

            // Searching with project number requires exact match.
            if (!string.IsNullOrWhiteSpace(searchParameters.FunderProjectNumber))
            {
                subQueries.Add(t => t.Term(fd => fd.FunderProjectNumber, searchParameters.FunderProjectNumber));
            }

            // When searching with FundedOrganisation, search from organization consortia Fi,Sv,En names.
            if (!string.IsNullOrWhiteSpace(searchParameters.FundedOrganization))
            {
                subQueries.Add(t => t.Nested(query => query
                    .Path("organizationConsortia")
                    .Query(q => q.MultiMatch(query => query
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields("organizationConsortia.nameFi, organizationConsortia.nameSv, organizationConsortia.nameEn")
                        .Query(searchParameters.FundedOrganization)
                    ))));
            }

            // Searching with funded organization id requires exact match.
            if (!string.IsNullOrWhiteSpace(searchParameters.FundedOrganizationId))
            {
                subQueries.Add(t => t.Nested(query => query
                    .Path("organizationConsortia.ids")
                    .Query(q => q.Term(term => term
                        .Field("organizationConsortia.ids.content")
                        .Value(searchParameters.FundedOrganizationId)
                    ))));
            }

            // Searching with funded person first name
            if (!string.IsNullOrWhiteSpace(searchParameters.FundedPersonFirstName))
            {
                subQueries.Add(t => t.Nested(query => query
                    .Path("fundingGroupPerson")
                    .Query(q => q.MultiMatch(query => query
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields("fundingGroupPerson.firstNames")
                        .Query(searchParameters.FundedPersonFirstName)
                    ))));
            }

            // Searching with funded person last name
            if (!string.IsNullOrWhiteSpace(searchParameters.FundedPersonLastName))
            {
                subQueries.Add(t => t.Nested(query => query
                    .Path("fundingGroupPerson")
                    .Query(q => q.MultiMatch(query => query
                        .Type(TextQueryType.PhrasePrefix)
                        .Fields("fundingGroupPerson.lastName")
                        .Query(searchParameters.FundedPersonLastName)
                    ))));
            }

            // Searching with funded organization id requires exact match.
            if (!string.IsNullOrWhiteSpace(searchParameters.FundedPersonOrcid))
            {
                subQueries.Add(t => t.Nested(query => query
                    .Path("fundingGroupPerson")
                    .Query(q => q.Term(term => term
                        .Field("fundingGroupPerson.orcId")
                        .Value(searchParameters.FundedPersonOrcid)
                    ))));
            }

            // Searching with type of funding id requires exact match.
            if (!string.IsNullOrWhiteSpace(searchParameters.TypeOfFunding))
            {
                subQueries.Add(t => t.Nested(query => query
                    .Path("typeOfFunding")
                    .Query(q => q.Term(term => term
                        .Field("typeOfFunding.typeId")
                        .Value(searchParameters.TypeOfFunding)
                    ))));
            }

            // Add date filter for FundingStartYearFrom
            filters.Add(x => x
                .Bool(b => b
                    .Should(s =>
                        (s.Range(r => r
                            .Field("fundingStartYear")
                            .GreaterThanOrEquals(searchParameters.FundingStartYearFrom ?? 0)
                            .LessThan(2100)))
                        ||
                        !s.Exists(b => b.Field("fundingStartYear"))
                        )));

            return searchDescriptor => searchDescriptor
                .Index(indexName)
                .Query(queryDescriptor => queryDescriptor
                    .Bool(boolDescriptor => boolDescriptor
                        .Must(subQueries)
                        .Filter(filters)));
        }
    }
}
