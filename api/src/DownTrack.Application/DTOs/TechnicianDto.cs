

namespace DownTrack.Application.DTO;

public class TechnicianDto
{
    public int Id {get;set;}
    public string Name { get; set; } = "name";
    public string Specialty { get; set; }="specialty";
    public double Salary { get; set; }
    public int ExpYears { get; set; }

}