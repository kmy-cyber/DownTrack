using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DownTrack.Application.IRepository;
using DownTrack.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using DownTrack.Domain.Entities;
using DownTrack.Application.Common.Authentication;
using DownTrack.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using DownTrack.Application.Authentication;
using DownTrack.Infrastructure.Initializer;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Infrastructure.UnitOfWorkPattern;


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

        service.AddScoped<IUnitOfWork, UnitOfWork>();



        // add infrastructure layer services
        var connectionString = configuration.GetConnectionString("AppDbConnectionString");
        var db = service.AddDbContext<DownTrackContext>(options => options.UseMySql(
                                                        connectionString, ServerVersion.AutoDetect(connectionString)));


        service.AddScoped<ITechnicianRepository, TechnicianRepository>();
        service.AddScoped<IEmployeeRepository, EmployeeRepository>();
        service.AddScoped<IUserRepository,UserRepository>();
        service.AddScoped<IEquipmentReceptorRepository, EquipmentReceptorRepository>();

        service.AddScoped<IEquipmentRepository, EquipmentRepository>();

        service.AddScoped<ISectionRepository, SectionRepository>();
      
        service.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
        
        service.AddScoped<IEvaluationRepository, EvaluationRepository>();
        

        // // Registering DownTrackContextInitializer as a scoped service. 
        // // It will be instantiated once per HTTP request, allowing it to manage database initialization for each request.
        // service.AddScoped<DownTrackContextInitializer>();


        service.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SECTION_NAME));

        service.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>(sp =>
    {
        var jwtSettings = sp.GetRequiredService<IOptions<JwtSettings>>().Value;
        return new JwtTokenGenerator(jwtSettings);
    });



        service.AddScoped<IIdentityManager, IdentityManager>();

        service.AddAuthentication();
        service
                .AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DownTrackContext>();

        
        //Register a service of type IHostedService in the dependency container
        service.AddHostedService<RoleInitializer>();


        service.AddScoped<IDepartmentRepository, DepartmentRepository>();

    }
}


//scoped : se crea una nueva instacnia por solicitud HTTP.Se
//utiliza para servicios que necesitan tener estados dentro de una
// solicitud, como el accede a la BD

//singleton: viven durante toda la vida de la app
// servicios que no tienen estado o que son costosos de crear, como configuracion o cache