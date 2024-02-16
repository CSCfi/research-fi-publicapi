namespace Serilog.Sinks.Http.TextFormatters;
public class WoodLogMetadataFormatterForIndexer : WoodLogMetadataFormatterBase
{
    // create constructor
    public WoodLogMetadataFormatterForIndexer()
    {
        IncludeMessageTemplate = false;
        IncludeRenderedMessage = true;
    }
}