using PruebaSCI.Application.DTOs.Weather;

namespace PruebaSCI.Application.Interfaces;

public interface IWeatherService
{
    Task<WeatherForecastResponse> GetForecastAsync(double latitude, double longitude, CancellationToken cancellationToken);
}
