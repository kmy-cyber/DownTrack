


using DownTrack.Application.IServices;
using DownTrack.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DownTrack.Application;


public static class DependencyInjection
{
    /// <summary>
    /// Adds application-specific services to the dependency injection container 
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static void AddAplication(this IServiceCollection service)
    {
        // add application layer services

        service.AddScoped<ITechnicianServices,TechnicianServices>();
    }
}