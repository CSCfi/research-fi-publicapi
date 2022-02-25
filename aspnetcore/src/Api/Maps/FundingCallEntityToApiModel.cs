using Api.Models.Entities;
using Api.Models.FundingCall;

namespace Api.Maps
{
    public class FundingCallEntityToApiModel : IMapper<DimCallProgramme, FundingCall>
    {
        public FundingCall Map(DimCallProgramme dbEntity)
        {
            var call = new FundingCall
            {
                NameFi = dbEntity.NameFi,
                NameSv = dbEntity.NameSv,
                NameEn = dbEntity.NameEn,
                DescriptionFi = dbEntity.DescriptionFi,
                DescriptionSv = dbEntity.DescriptionSv,
                DescriptionEn = dbEntity.DescriptionEn,
                ApplicationTermsFi = dbEntity.ApplicationTermsFi,
                ApplicationTermsSv = dbEntity.ApplicationTermsSv,
                ApplicationTermsEn = dbEntity.ApplicationTermsEn,
                ContactInformation = dbEntity.ContactInformation,
                CallProgrammeOpenDate = Convert(dbEntity.DimDateIdOpenNavigation),
                // TODO: need to check for default datetime?
                CallProgrammeDueDate = Convert(dbEntity.DimDateIdDueNavigation),
                ApplicationURLFi = GetUrl(dbEntity.DimWebLinks, "ApplicationUrl", "fi"),
                ApplicationURLSv = GetUrl(dbEntity.DimWebLinks, "ApplicationUrl", "sv"),
                ApplicationURLEn = GetUrl(dbEntity.DimWebLinks, "ApplicationUrl", "en"),
                Foundation = GetFoundations(dbEntity.DimOrganizations),
                Categories = GetCategories(dbEntity.DimReferencedata)
            };

            return call;
        }
        private static DateTime? Convert(DimDate? dimDate)
        {
            if (dimDate == null)
            {
                return null;
            }

            return new DateTime(dimDate.Year, dimDate.Month, dimDate.Day);
        }

        private static string? GetUrl(ICollection<DimWebLink> webLinks, string linkType, string? language = null)
        {
            if (webLinks == null)
            {
                return null;
            }

            var urlsWithType = webLinks.Where(l => l.LinkType == linkType);

            if (language == null)
            {
                return urlsWithType.SingleOrDefault()?.Url;
            }

            return urlsWithType.SingleOrDefault(l => l.LanguageVariant == language)?.Url;
        }

        private static Foundation[] GetFoundations(ICollection<DimOrganization> organizations)
        {
            return organizations.Select(o => new Foundation
            {
                FoundationNameFi = o.NameFi,
                FoundationNameSv = o.NameSv,
                FoundationNameEn = o.NameEn,
                FoundationBusinessId = o.OrganizationId,
                FoundationUrl = GetUrl(o.DimWebLinks, "FoundationURL"),
            }).ToArray();
        }

        private static Category[] GetCategories(ICollection<DimReferencedatum> referenceData)
        {
            return referenceData.Select(r => new Category
            {
                CategoryCodeValue = r.CodeValue,
                CategoryNameFi = r.NameFi,
                CategoryNameSv = r.NameSv,
                CategoryNameEn = r.NameEn,
            }).ToArray();
        }
    }
}
