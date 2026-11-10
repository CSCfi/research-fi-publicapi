using System.Text.Json;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.Http.TextFormatters;
using Xunit;

namespace CSC.PublicApi.Indexer.Tests.Logging;

public class WoodLogMetadataFormatterTest
{
    [Fact]
    public void InterfaceFormatter_EmitsNeitherMessageFormAndRoutesInterfaceMetadata()
    {
        var properties = new[]
        {
            new LogEventProperty("Application", new ScalarValue("ResearchFi")),
            new LogEventProperty("WoodLogProjectNumber", new ScalarValue("1234")),
            new LogEventProperty("WoodLogUseCaseInterface", new ScalarValue("interface-use-case")),
            new LogEventProperty("WoodLogRetentionMonthsInterface", new ScalarValue("6")),
            new LogEventProperty("WoodLogUseCaseIndexer", new ScalarValue("indexer-use-case"))
        };
        var logEvent = CreateLogEvent("Request {RequestId}", properties);
        using var output = new StringWriter();

        new WoodLogMetadataFormatterForInterface().Format(logEvent, output);

        using var document = JsonDocument.Parse(output.ToString());
        var root = document.RootElement;
        Assert.False(root.TryGetProperty("MessageTemplate", out _));
        Assert.False(root.TryGetProperty("RenderedMessage", out _));
        Assert.Equal("ResearchFi", root.GetProperty("Properties").GetProperty("Application").GetString());
        Assert.False(root.GetProperty("Properties").TryGetProperty("WoodLogProjectNumber", out _));
        Assert.Equal(
            "indexer-use-case",
            root.GetProperty("Properties").GetProperty("WoodLogUseCaseIndexer").GetString());
        var wood = root.GetProperty("wood");
        Assert.Equal("1234", wood.GetProperty("project_number").GetString());
        Assert.Equal("interface-use-case", wood.GetProperty("use_case").GetString());
        Assert.Equal("6", wood.GetProperty("retention_months").GetString());
    }

    [Fact]
    public void IndexerFormatter_EmitsRenderedMessageAndIndexerMetadata()
    {
        var properties = new[]
        {
            new LogEventProperty("Count", new ScalarValue(3)),
            new LogEventProperty("WoodLogProjectNumber", new ScalarValue("5678")),
            new LogEventProperty("WoodLogUseCaseInterface", new ScalarValue("interface-use-case")),
            new LogEventProperty("WoodLogUseCaseIndexer", new ScalarValue("indexer-use-case")),
            new LogEventProperty("WoodLogRetentionMonthsIndexer", new ScalarValue("12"))
        };
        var logEvent = CreateLogEvent("Indexed {Count} records", properties);
        using var output = new StringWriter();

        new WoodLogMetadataFormatterForIndexer().Format(logEvent, output);

        using var document = JsonDocument.Parse(output.ToString());
        var root = document.RootElement;
        Assert.False(root.TryGetProperty("MessageTemplate", out _));
        Assert.Equal("Indexed 3 records", root.GetProperty("RenderedMessage").GetString());
        Assert.Equal(3, root.GetProperty("Properties").GetProperty("Count").GetInt32());
        Assert.Equal(
            "interface-use-case",
            root.GetProperty("Properties").GetProperty("WoodLogUseCaseInterface").GetString());
        var wood = root.GetProperty("wood");
        Assert.Equal("5678", wood.GetProperty("project_number").GetString());
        Assert.Equal("indexer-use-case", wood.GetProperty("use_case").GetString());
        Assert.Equal("12", wood.GetProperty("retention_months").GetString());
    }

    [Fact]
    public void Formatter_IncludesExceptionDetailsAsJsonString()
    {
        var logEvent = CreateLogEvent("Operation failed", Array.Empty<LogEventProperty>(),
            new InvalidOperationException("synthetic failure"));
        using var output = new StringWriter();

        new WoodLogMetadataFormatterForInterface().Format(logEvent, output);

        using var document = JsonDocument.Parse(output.ToString());
        Assert.Contains("synthetic failure", document.RootElement.GetProperty("Exception").GetString());
    }

    private static LogEvent CreateLogEvent(
        string messageTemplate,
        IEnumerable<LogEventProperty> properties,
        Exception? exception = null) =>
        new(
            DateTimeOffset.Parse("2026-01-02T03:04:05Z"),
            LogEventLevel.Information,
            exception,
            new MessageTemplateParser().Parse(messageTemplate),
            properties);
}
