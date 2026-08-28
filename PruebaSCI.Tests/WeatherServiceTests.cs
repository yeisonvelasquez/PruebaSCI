using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using PruebaSCI.Application.Options;
using PruebaSCI.Infrastructure.ExternalServices;

namespace PruebaSCI.Tests;

public sealed class WeatherServiceTests
{
    [Fact]
    public async Task GetForecastAsync_MapsOpenMeteoResponse()
    {
        using var httpClient = new HttpClient(new StubHttpMessageHandler())
        {
            BaseAddress = new Uri("https://api.open-meteo.com/")
        };
        var service = new WeatherService(
            httpClient,
            Options.Create(new OpenMeteoOptions
            {
                BaseUrl = "https://api.open-meteo.com/",
                ForecastPath = "v1/forecast"
            }));

        var result = await service.GetForecastAsync(52.52, 13.41, CancellationToken.None);

        Assert.Equal(52.52, result.Latitude);
        Assert.Equal(13.41, result.Longitude);
        Assert.Equal([20.1, 21.2], result.Hourly["temperature_2m"]);
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new
                {
                    latitude = 52.52,
                    longitude = 13.41,
                    hourly = new { temperature_2m = new[] { 20.1, 21.2 } }
                })
            };

            return Task.FromResult(response);
        }
    }
}
