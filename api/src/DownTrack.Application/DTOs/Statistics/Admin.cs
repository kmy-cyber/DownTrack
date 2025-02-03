
namespace DownTrack.Application.DTO.Statistics;

public class AdminStatisticsDto
{
    public int NumberEmployee {get;set;}
    public int NumberSections {get;set;}
    public int NumberDepartments {get;set;}
    public Dictionary<string,int> RolesStatistics {get;set;} = null!;
    public Dictionary<string,int> DepartmentsByMonth {get;set;}= null!;
    public Dictionary<string,int> SectionsByMonth {get;set;} = null!;
    
    //public Dictionary<string,(int,int)> InventaryByMonths {get;set;} = null!;
}