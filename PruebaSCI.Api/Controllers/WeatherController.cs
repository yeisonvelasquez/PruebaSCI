using Microsoft.AspNetCore.Mvc;
using PruebaSCI.Application.DTOs.Weather;
using PruebaSCI.Application.Interfaces;

namespace PruebaSCI.Api.Controllers;

[ApiController]
[Route("api/weather")]
public sealed class WeatherController(IWeatherService weatherService) : ControllerBase
{
    /// <summary>Consulta el pronóstico horario de Open-Meteo.</summary>
    /// <param name="latitude">Latitud entre -90 y 90.</param>
    /// <param name="longitude">Longitud entre -180 y 180.</param>
    /// <param name="cancellationToken">Token para cancelar la solicitud.</param>
    /// <response code="200">El pronóstico fue obtenido correctamente.</response>
    /// <response code="400">Las coordenadas no son válidas.</response>
    /// <response code="502">El servicio meteorológico no está disponible.</response>
    [ProducesResponseType(typeof(WeatherForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    [HttpGet]
    public async Task<ActionResult<WeatherForecastResponse>> GetForecast(
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        CancellationToken cancellationToken)
    {
        if (latitude is < -90 or > 90 || longitude is < -180 or > 180)
        {
            return BadRequest(new { message = "La latitud debe estar entre -90 y 90, y la longitud entre -180 y 180." });
        }

        return Ok(await weatherService.GetForecastAsync(latitude, longitude, cancellationToken));
    }
}
