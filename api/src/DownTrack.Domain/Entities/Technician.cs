

namespace DownTrack.Domain.Enitites;

public class Technician : User
{
    public string Specialty { get; set; }="specialty";
    public double Salary { get; set; }
    public int ExpYears { get; set; }

}