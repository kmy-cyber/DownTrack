
namespace DownTrack.Domain.Enum;

public class UserRole
{
    public const string Administrator = "Administrator";
    public const string SectionManager = "SectionManager";
    public const string Technician = "Technician";
    public const string EquipmentReceptor = "EquipmentReceptor";
    public const string Director = "Director";
    public const string ShippingSupervisor = " ShippingSupervisor";


    public static readonly string[] Roles =
        {
            "Administrator",
            "SectionManager",
            "Technician",
            "EquipmentReceptor",
            "Director",
            "ShippingSupervisor"
        };


    public static bool IsValidRole(string role)
    {
        return Roles.Contains(role);
    }            

}


// public enum UserRole
// {
//      Administrator = "Administrador",
//      SectionManager = "SectionManager",
//      Technician = "Tecnico",
//      EquipmentReceptor ="EquipmentReceptor",
//      Director = "Director",
//      ShippingSupervisor = "ShippingSupervisor"

// }
