using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DownTrack.Infrastructure.Repository;
using DownTrack.Application.IRepository;


namespace DownTrack.Infrastructure;


public static class DependencyInjection
{
    /// <summary>
    /// Adds application-specific services to the dependency injection container 
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The modified IServiceCollection.</returns>

    public static void AddInfrastructure(this IServiceCollection service, IConfiguration configuration)

    {
        // add infrastructure layer services
        var connectionString = configuration.GetConnectionString("AppDbConnectionString");
        var db = service.AddDbContext<DownTrackContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        service.AddScoped<ITechnicianRepository, TechnicianRepository>();
        service.AddScoped<IEquipmentRepository, EquipmentRepository>();
        service.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
    }
}