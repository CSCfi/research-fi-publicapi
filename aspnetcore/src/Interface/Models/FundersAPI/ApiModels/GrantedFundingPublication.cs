using System.ComponentModel.DataAnnotations;

namespace CSC.PublicApi.Interface.Models.FundersAPI.ApiModels
{
    public partial class GrantedFundingPublication
    {
        public string? PublicationId { get; set; }
        public string? Doi { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(PublicationId) && string.IsNullOrEmpty(Doi))
            {
                yield return new ValidationResult(
                    "Either PublicationId or Doi must be provided.",
                    new[] { nameof(PublicationId), nameof(Doi) });
            }
        }

        public string GetPublicationIdentifier()
        {
            if (!string.IsNullOrEmpty(PublicationId))
            {
                return PublicationId;
            }
            else if (!string.IsNullOrEmpty(Doi))
            {
                return Doi;
            }
            else
            {
                throw new InvalidOperationException("Neither PublicationId nor Doi is set.");
            }
        }
    }
}
