using System.ComponentModel.DataAnnotations;

namespace CSC.PublicApi.Interface.Models.FundersAPI.ApiModels
{
    public partial class GrantedFundingPublication
    {
        [Required(ErrorMessage = "Either PublicationId or Doi must be provided.")]
        public string? PublicationId { get; set; }

        [Required(ErrorMessage = "Either PublicationId or Doi must be provided.")]
        public string? Doi { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(PublicationId) || !string.IsNullOrEmpty(Doi);
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
