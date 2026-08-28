namespace PruebaSCI.Application.DTOs.Weather;

public sealed record WeatherForecastResponse(
    double Latitude,
    double Longitude,
    IReadOnlyDictionary<string, IReadOnlyList<double>> Hourly);
