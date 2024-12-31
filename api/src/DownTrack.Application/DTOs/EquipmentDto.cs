using System.Text.Json.Serialization;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;

namespace DownTrack.Application.DTO
{
    public class EquipmentDto
    {
        public int Id {get;set;}
        public string Name { get; set; } = "name";
        public string Type { get; set; } = "type";
        public EquipmentStatus Status { get; set; }
        public DateTime DateOfadquisition { get; set; } = DateTime.Now;
        public EquipmentDecommissioning? EquipmentDecommissioning { get; set; }

    }
}