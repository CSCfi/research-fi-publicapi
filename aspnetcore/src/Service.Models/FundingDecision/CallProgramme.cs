using Nest;

namespace CSC.PublicApi.Service.Models.FundingDecision;

public class CallProgramme
{
    [Ignore]
    public int  Id { get; set; }
    public string? SourceId { get; set; }
    public string? NameFi { get; set; }
    public string? NameSv { get; set; }
    public string? NameEn { get; set; }

    [Ignore]
    public string? EuCallId { get; set; }
    
    [Ignore]
    public string? Abbreviation { get; set; }
    
    [Ignore]
    public string? SourceDescription { get; set; }

    [Ignore]
    public string? SourceProgrammeId { get; set; }

    public FrameworkProgramme ToFrameworkProgramme()
    {
        return new FrameworkProgramme
        {
            NameFi = NameFi,
            NameEn = NameEn,
            NameSv = NameSv
        };
    }
}