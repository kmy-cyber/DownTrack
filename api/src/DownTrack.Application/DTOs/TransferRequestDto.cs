namespace DownTrack.Application.DTO;

public class TransferRequestDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int EmployeeId { get; set; }
    public int EquipmentId { get; set; }
    public int DepartmentId { get; set; }

    // public required string DepartamentoName { get; set; } 
    // public required string  SeccionName { get; set; }
}

