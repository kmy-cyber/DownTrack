
using Microsoft.Extensions.DependencyInjection;

namespace DownTrack.Infrastructure;


public static class DependencyInjection
{
    /// <summary>
    /// Adds application-specific services to the dependency injection container 
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The modified IServiceCollection.</returns>

    public static IServiceCollection AddInfrastructure(this IServiceCollection service)
    {
        // add infrastructure layer services

        return service;
    }
}