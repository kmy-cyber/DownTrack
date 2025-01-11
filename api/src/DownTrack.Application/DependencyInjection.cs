

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
    public static void AddAplication(this IServiceCollection service, ConfigurationManager configurationManager)
    {
        // add application layer services
        service.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        service.AddScoped<ITechnicianServices, TechnicianServices>();
        service.AddScoped<IEmployeeServices, EmployeeServices>();


        service.AddScoped<IIdentityService,IdentityService>();
        
      
        service.AddScoped<IEquipmentServices, EquipmentServices>();

        service.AddScoped<ISectionServices, SectionServices>();
      

        service.AddScoped<IDepartmentServices, DepartmentServices>();

    }
}