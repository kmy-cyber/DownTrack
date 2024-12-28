

namespace DownTrack.Domain.Enitites;

public class Technician : User
{
    public string Specialty { get; set; } = string.Empty;
    public double Salary { get; set; }
    public int ExpYears { get; set; }

}