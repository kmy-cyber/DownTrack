
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

}



