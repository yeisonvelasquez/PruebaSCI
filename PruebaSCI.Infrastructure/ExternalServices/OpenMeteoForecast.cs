using System.Text.Json.Serialization;

namespace PruebaSCI.Infrastructure.ExternalServices;

internal sealed class OpenMeteoForecast
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; init; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; init; }

    [JsonPropertyName("hourly")]
    public OpenMeteoHourlyData Hourly { get; init; } = new();
}

internal sealed class OpenMeteoHourlyData
{
    [JsonPropertyName("temperature_2m")]
    public double[] Temperature2m { get; init; } = [];
}
