namespace DownTrack.Application.DTO
{
    public class EquipmentDto
    {
        public int Id {get;set;}
        public string Name { get; set; } = "name";
        public string Type { get; set; } = "type";
        public string Status { get; set; } = "status";
        public DateTime DateOfadquisition { get; set; } = DateTime.Now;
    }
}