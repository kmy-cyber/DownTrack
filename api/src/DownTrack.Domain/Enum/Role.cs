
namespace DownTrack.Domain.Roles;

public enum UserRole
{
    Administrator,
    SectionManager,
    Technician,
    EquipmentReceptor,
    Director,
    ShippingSupervisor
}

public static class UserRoleHelper
{
    /// <summary>
    /// Validates whether the specified role belongs to any of the registered roles
    /// </summary>
    public static bool IsValidRole(string role) =>
        System.Enum.TryParse(typeof(UserRole), role, out _);

    /// <summary>
    /// Retrieves all roles defined in the UserRole enum as a collection of strings.
    /// </summary>
    /// <returns>A read-only collection containing all role names.</returns>
    public static IReadOnlyCollection<string> AllRoles() =>
        System.Enum.GetNames(typeof(UserRole));

    /// <summary>
    /// Converts a string representation of a role to its corresponding UserRole enum value.
    /// </summary>
    /// <param name="role">The role name as a string.</param>
    /// <returns>The UserRole enum value if the conversion is successful; otherwise, null.</returns>
    public static UserRole? FromString(string role) =>
        System.Enum.TryParse(typeof(UserRole), role, out var result) ? (UserRole?)result : null;

    /// <summary>
    /// Converts a UserRole enum value to its string representation.
    /// </summary>
    /// <param name="role">The UserRole enum value to convert.</param>
    /// <returns>The string representation of the provided UserRole.</returns>
    public static string ToString(UserRole role) => role.ToString();
}





// public class UserRole
// {
//     public const string Administrator = "Administrator";
//     public const string SectionManager = "SectionManager";
//     public const string Technician = "Technician";
//     public const string EquipmentReceptor = "EquipmentReceptor";
//     public const string Director = "Director";
//     public const string ShippingSupervisor = " ShippingSupervisor";


//     public static readonly string[] Roles =
//         {
//             "Administrator",
//             "SectionManager",
//             "Technician",
//             "EquipmentReceptor",
//             "Director",
//             "ShippingSupervisor"
//         };


//     public static bool IsValidRole(string role)
//     {
//         return Roles.Contains(role);
//     }            

// }
