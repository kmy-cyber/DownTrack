
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DownTrack.Application.IRespository;
using DownTrack.Infrastructure.Repository;
using DownTrack.Infrastructure.Authentication;

namespace DownTrack.Infrastructure;


public static class DependencyInjection
{
    /// <summary>
    /// Adds application-specific services to the dependency injection container 
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The modified IServiceCollection.</returns>

    public static void AddInfrastructure(this IServiceCollection service, ConfigurationManager configuration)

    {
        // add infrastructure layer services
        var connectionString = configuration.GetConnectionString("AppDbConnectionString");
        var db = service.AddDbContext<DownTrackContext>(options => options.UseMySql(
                                                        connectionString, ServerVersion.AutoDetect(connectionString)));

        service.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SECTION_NAME));
        service.AddScoped<ITechnicianRepository, TechnicianRepository>();
        service.AddScoped<IUserRepository, UserRepository>();

    }
}