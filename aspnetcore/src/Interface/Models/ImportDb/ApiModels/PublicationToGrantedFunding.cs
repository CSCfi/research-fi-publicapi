using System.ComponentModel.DataAnnotations;

namespace CSC.PublicApi.Interface.Models.ImportDb.ApiModels;

public partial class PublicationToGrantedFunding : IValidatableObject
{
    public string? PublicationId { get; set; }
    public string? Doi { get; set; }
    public string OrganizationId { get; set; } = null!;
    public string FunderProjectNumber { get; set; } = null!;

    // Add custom model validation. Either PublicationId or Doi must be provided.
    // This enables controller to validate the model before it is passed to the service layer.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(PublicationId) && string.IsNullOrWhiteSpace(Doi))
        {
            yield return new ValidationResult(
                "At least one of PublicationId or Doi is required.",
                new[] { nameof(PublicationId), nameof(Doi) });
        }
    }
}
