using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PruebaSCI.Application.Interfaces;
using PruebaSCI.Application.Options;
using PruebaSCI.Infrastructure.ExternalServices;
using PruebaSCI.Infrastructure.Repositories;

namespace PruebaSCI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenMeteoOptions>(configuration.GetSection(OpenMeteoOptions.SectionName));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddHttpClient<IWeatherService, WeatherService>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<OpenMeteoOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        return services;
    }
}
