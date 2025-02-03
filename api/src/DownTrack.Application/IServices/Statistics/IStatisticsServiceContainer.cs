using DownTrack.Application.IServices.Statistics;

public class StatisticsServicesContainer
{
    public IAdminStatisticsService AdminStatisticsService { get; }
    public ITechnicianStatisticsService TechnicianStatisticsService { get; }
    public IReceptorStatisticsService ReceptorStatisticsService { get; }
    public IDirectorStatisticsService DirectorStatisticsService { get; }
    public ISectionManagerStatisticsService SectionManagerStatisticsService { get; }

    public StatisticsServicesContainer(
        IAdminStatisticsService adminStatisticsService,
        ITechnicianStatisticsService technicianStatisticsService,
        IReceptorStatisticsService receptorStatisticsService,
        IDirectorStatisticsService directorStatisticsService,
        ISectionManagerStatisticsService sectionManagerStatisticsService)
    {
        AdminStatisticsService = adminStatisticsService;
        TechnicianStatisticsService = technicianStatisticsService;
        ReceptorStatisticsService = receptorStatisticsService;
        DirectorStatisticsService = directorStatisticsService;
        SectionManagerStatisticsService = sectionManagerStatisticsService;
    }
}
