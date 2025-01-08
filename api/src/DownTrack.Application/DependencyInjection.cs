

using DownTrack.Application.IServices;
using DownTrack.Application.IServices.Authentication;
using DownTrack.Application.Services;
using DownTrack.Application.Services.Authentication;
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
    public static IServiceCollection AddAplication(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        // Registers AutoMapper to enable mapping between DTOs and domain models.
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Registers services related to Entities
        services.AddScoped<ITechnicianServices, TechnicianServices>();
        services.AddScoped<IEmployeeServices, EmployeeServices>();
        services.AddScoped<IMaintenanceServices,MaintenanceServices>();
        services.AddScoped<IIdentityService,IdentityService>();
        services.AddScoped<IEquipmentServices, EquipmentServices>();
        services.AddScoped<ISectionServices, SectionServices>();
        services.AddScoped<IDepartmentServices, DepartmentServices>();


        return services;
    }
}