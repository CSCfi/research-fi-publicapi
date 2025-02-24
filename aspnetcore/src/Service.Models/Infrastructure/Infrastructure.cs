namespace CSC.PublicApi.Service.Models.Infrastructure;

public class Infrastructure
{
    public string? NameFi { get; set; }
    public string? NameSv { get; set; }
    public string? NameEn { get; set; }

    /// <summary>
    /// Linkki Tutkimustietovarantoon
    /// </summary>
    public ResearchfiUrl? ResearchfiUrl { get; set; }
}