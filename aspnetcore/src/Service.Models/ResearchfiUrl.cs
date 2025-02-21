namespace CSC.PublicApi.Service.Models;

public class ResearchfiUrl
{
    private const string ResearchfiBaseUrlFi = "https://researchfi-devel.2.rahtiapp.fi/fi/results/"; // "https://tiedejatutkimus.fi/fi/results/";
    private const string ResearchfiBaseUrlEn = "https://researchfi-devel-en.2.rahtiapp.fi/en/results/"; // "https://research.fi/en/results/";
    private const string ResearchfiBaseUrlSv ="https://researchfi-devel-sv.2.rahtiapp.fi/sv/results/"; // "https://forskning.fi/sv/results/";
  
    public ResearchfiUrl(string resourceType, string? id)
    {
        Fi = string.Empty;
        Sv = string.Empty;
        En = string.Empty;

        if (!string.IsNullOrWhiteSpace(id))
        {
            string idEscaped = Uri.EscapeDataString(id.ToString());

            if (resourceType == "funding-call")
            {
                Fi = $"{ResearchfiBaseUrlFi}funding-call/{idEscaped}";
                Sv = $"{ResearchfiBaseUrlSv}funding-call/{idEscaped}";
                En = $"{ResearchfiBaseUrlEn}funding-call/{idEscaped}";
            }
            else if (resourceType == "funding-decision")
            {
                Fi = $"{ResearchfiBaseUrlFi}funding/{idEscaped}";
                Sv = $"{ResearchfiBaseUrlSv}funding/{idEscaped}";
                En = $"{ResearchfiBaseUrlEn}funding/{idEscaped}";
            }
            else if (resourceType == "publication")
            {
                Fi = $"{ResearchfiBaseUrlFi}publication/{idEscaped}";
                Sv = $"{ResearchfiBaseUrlSv}publication/{idEscaped}";
                En = $"{ResearchfiBaseUrlEn}publication/{idEscaped}";
            }
            else if (resourceType == "research-dataset")
            {
                Fi = $"{ResearchfiBaseUrlFi}dataset/{idEscaped}";
                Sv = $"{ResearchfiBaseUrlSv}dataset/{idEscaped}";
                En = $"{ResearchfiBaseUrlEn}dataset/{idEscaped}";
            }
        }
    }

    public string Fi { get; set; }
    public string Sv { get; set; }
    public string En { get; set; }
}