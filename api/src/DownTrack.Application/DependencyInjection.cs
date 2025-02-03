using DownTrack.Application.IServices;
using DownTrack.Application.IServices.Authentication;
using DownTrack.Application.IServices.Statistics;
using DownTrack.Application.Services;
using DownTrack.Application.Services.Authentication;
using DownTrack.Application.Services.Statistics;
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

        services.AddScoped<IIdentityService,IdentityService>();

        services.AddScoped<IEmployeeQueryServices,EmployeeQueryServices>();
        services.AddScoped<IEmployeeCommandServices,EmployeeCommandServices>();
        services.AddScoped<ISectionQueryServices,SectionQueryServices>();
        services.AddScoped<ISectionCommandServices,SectionCommandServices>();
        services.AddScoped<IDepartmentQueryServices, DepartmentQueryServices>();
        services.AddScoped<IDepartmentCommandServices, DepartmentCommandServices>();
        services.AddScoped<ITechnicianQueryServices, TechnicianQueryServices>();
        services.AddScoped<ITechnicianCommandServices, TechnicianCommandServices>();
        services.AddScoped<IEquipmentReceptorQueryServices, EquipmentReceptorQueryServices>();
        services.AddScoped<IEquipmentReceptorCommandServices, EquipmentReceptorCommandServices>();
        services.AddScoped<IEvaluationQueryServices, EvaluationQueryServices>();
        services.AddScoped<IEvaluationCommandServices, EvaluationCommandServices>();
        services.AddScoped<IDoneMaintenanceCommandServices, DoneMaintenanceCommandServices>();
        services.AddScoped<IDoneMaintenanceQueryServices, DoneMaintenanceQueryServices>();
        services.AddScoped<IEquipmentCommandServices, EquipmentCommandServices>();
        services.AddScoped<IEquipmentQueryServices, EquipmentQueryServices>();
        services.AddScoped<IEquipmentDecommissioningCommandServices, EquipmentDecommissioningCommandServices>();
        services.AddScoped<IEquipmentDecommissioningQueryServices, EquipmentDecommissioningQueryServices>();
        services.AddScoped<ITransferRequestCommandServices, TransferRequestCommandServices>();
        services.AddScoped<ITransferRequestQueryServices, TransferRequestQueryServices>();
        services.AddScoped<ITransferCommandServices, TransferCommandServices>();
        services.AddScoped<ITransferQueryServices, TransferQueryServices>();
        
        services.AddScoped<IAdminStatisticsService, AdminStatisticsService>();
        services.AddScoped<ITechnicianStatisticsService, TechnicianStatisticsService>();
        services.AddScoped<IReceptorStatisticsService, ReceptorStatisticsService>();
        services.AddScoped<IDirectorStatisticsService, DirectorStatisticsService>();
        services.AddScoped<ISectionManagerStatisticsService, SectionManagerStatisticsService>();

        services.AddScoped<StatisticsServicesContainer>();
        
        return services;


    }
}