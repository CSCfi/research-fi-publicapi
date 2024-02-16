namespace Serilog.Sinks.Http.TextFormatters;
public class WoodLogMetadataFormatterForInterface : WoodLogMetadataFormatterBase
{
    // create constructor
    public WoodLogMetadataFormatterForInterface()
    {
        IncludeMessageTemplate = false;
        IncludeRenderedMessage = false;
    }
}