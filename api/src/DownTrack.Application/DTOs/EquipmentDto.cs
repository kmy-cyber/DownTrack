using DownTrack.Domain.Enum;

namespace DownTrack.Application.DTO
{
    public class EquipmentDto
    {
        public int Id {get;set;}
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public EquipmentStatus EquipmentStatus { get; set; } = EquipmentStatus.Active;
        public DateTime DateOfadquisition { get; set; } = DateTime.Now;
    }
}