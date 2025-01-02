
namespace DownTrack.Domain.Entities;

public class Technician : GenericEntity
{
    public string Name { get; set; } = null!;
    public string Specialty { get; set; }=null!;
    public double Salary { get; set; }
    public int ExpYears { get; set; }
    public ICollection<EquipmentDecommissioning>? EquipmentDecommissionings { get; set; }

}