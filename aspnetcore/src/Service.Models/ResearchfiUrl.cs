using CSC.PublicApi.Service.Models.Infrastructure;

namespace CSC.PublicApi.Service.Models;

public class ResearchfiUrl
{
    private const string ResearchfiBaseUrlFi = "https://tiedejatutkimus.fi/fi/results/";
    private const string ResearchfiBaseUrlEn = "https://research.fi/en/results/";
    private const string ResearchfiBaseUrlSv = "https://forskning.fi/sv/results/";
  
    public ResearchfiUrl(string resourceType, string? id, string? infrastructureServiceId = null)
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
            else if (resourceType == "infrastructure")
            {
                // https://wiki.eduuni.fi/x/pSHrJw
                // 
                // Example:
                // Infrastructure URN = "urn:nbn:fi:ttv-202509000687197"
                // =>
                // "https://research.fi/en/results/infrastructure/ttv-202509000687197"
                if (id.Contains(':'))
                {
                    id = id.Split(':')[^1];
                    idEscaped = Uri.EscapeDataString(id.ToString());
                }
                Fi = $"{ResearchfiBaseUrlFi}infrastructure/{idEscaped}";
                Sv = $"{ResearchfiBaseUrlSv}infrastructure/{idEscaped}";
                En = $"{ResearchfiBaseUrlEn}infrastructure/{idEscaped}";
            }
            else if (resourceType == "infrastructure-service")
            {
                // https://wiki.eduuni.fi/x/pSHrJw
                //
                // Example:
                // Infrastructure URN = "urn:nbn:fi:ttv-202509000687197"
                // Infrastructure Service URN = "urn:nbn:fi:ttv-202509000687180"
                // =>
                // "https://research.fi/en/results/infrastructure/ttv-202509000687197?service=ttv-202509000687180"
                if (id.Contains(':'))
                {
                    id = id.Split(':')[^1];
                    idEscaped = Uri.EscapeDataString(id.ToString());
                }
                if (infrastructureServiceId.Contains(':'))
                {
                    infrastructureServiceId = infrastructureServiceId.Split(':')[^1];
                    infrastructureServiceId = Uri.EscapeDataString(infrastructureServiceId.ToString());
                }
                Fi = $"{ResearchfiBaseUrlFi}infrastructure/{idEscaped}?service={infrastructureServiceId}";
                Sv = $"{ResearchfiBaseUrlSv}infrastructure/{idEscaped}?service={infrastructureServiceId}";
                En = $"{ResearchfiBaseUrlEn}infrastructure/{idEscaped}?service={infrastructureServiceId}";
            }
        }
    }

    public string Fi { get; set; }
    public string Sv { get; set; }
    public string En { get; set; }
}