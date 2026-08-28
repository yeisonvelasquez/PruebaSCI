using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PruebaSCI.Application.DTOs.Weather;
using PruebaSCI.Application.Interfaces;
using PruebaSCI.Application.Options;

namespace PruebaSCI.Infrastructure.ExternalServices;

public sealed class WeatherService(HttpClient httpClient, IOptions<OpenMeteoOptions> options) : IWeatherService
{
    private readonly OpenMeteoOptions settings = options.Value;

    public async Task<WeatherForecastResponse> GetForecastAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken)
    {
        var query = $"{settings.ForecastPath}?latitude={latitude}&longitude={longitude}&hourly=temperature_2m";
        using var response = await httpClient.GetAsync(query, cancellationToken);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        var forecastElement = payload.ValueKind == JsonValueKind.Array
            ? payload.EnumerateArray().FirstOrDefault()
            : payload;
        var forecast = forecastElement.ValueKind == JsonValueKind.Undefined
            ? null
            : forecastElement.Deserialize<OpenMeteoForecast>();
        if (forecast is null)
        {
            throw new InvalidOperationException("La respuesta del servicio meteorológico está vacía.");
        }

        return new WeatherForecastResponse(
            forecast.Latitude,
            forecast.Longitude,
            new Dictionary<string, IReadOnlyList<double>>
            {
                ["temperature_2m"] = forecast.Hourly.Temperature2m
            });
    }
}
