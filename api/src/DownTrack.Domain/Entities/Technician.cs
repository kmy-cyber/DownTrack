

namespace DownTrack.Domain.Entities;

public class Technician : Employee
{
    public string Specialty { get; set; } = null!;
    public double Salary { get; set; }
    public int ExpYears { get; set; }
  
}