

using DownTrack.Application.IServices;
using DownTrack.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DownTrack.Application;


public static class DependencyInjection
{
    /// <summary>
    /// Adds application-specific services to the dependency injection container 
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static void AddAplication(this IServiceCollection service, ConfigurationManager configurationManager)
    {
        // add application layer services
        service.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        service.AddScoped<ITechnicianServices,TechnicianServices>();
        service.AddScoped<IEquipmentServices, EquipmentServices>();
        service.AddScoped<ISectionServices, SectionServices>();
    }
}