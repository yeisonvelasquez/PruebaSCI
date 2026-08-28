using Microsoft.Extensions.DependencyInjection;
using PruebaSCI.Application.Interfaces;
using PruebaSCI.Application.Services;

namespace PruebaSCI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}
