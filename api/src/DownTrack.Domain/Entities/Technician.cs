

using System.Text.Json.Serialization;

namespace DownTrack.Domain.Entities;

public class Technician : GenericEntity
{
    public string Name { get; set; } = "name";
    public string Specialty { get; set; }="specialty";
    public double Salary { get; set; }
    public int ExpYears { get; set; }

    [JsonIgnore]
    public ICollection<EquipmentDecommissioning> EquipmentDecommissionings { get; set; } = new List<EquipmentDecommissioning>();

}