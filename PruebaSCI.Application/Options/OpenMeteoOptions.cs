namespace PruebaSCI.Application.Options;

public sealed class OpenMeteoOptions
{
    public const string SectionName = "OpenMeteo";

    public string BaseUrl { get; init; } = string.Empty;
    public string ForecastPath { get; init; } = string.Empty;
}
