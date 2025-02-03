// Summary:
// This file contains a utility class for mapping user roles to entity types.

using DownTrack.Domain.Entities;
using DownTrack.Domain.Roles;

namespace DownTrack.Domain.Const;

public static class GetEntity
{
    /// <summary>
    /// Retrieves the entity type associated with a given role.
    /// </summary>
    /// <param name="role">The role to map.</param>
    /// <returns>The corresponding entity type, or null if not found.</returns>
    public static Type? GetEntityTypeForRole(string role)
    {
        // Role mappings dictionary
        var roleMappings = new Dictionary<string, Type>
    {
        { UserRole.Technician.ToString(), typeof(Technician) },
        { UserRole.EquipmentReceptor.ToString(), typeof(EquipmentReceptor) },
        {"Employee", typeof(Employee)}
    };
        // Return the mapped type if exists, otherwise return null
        return roleMappings.ContainsKey(role) ? roleMappings[role] : null;
    }
}
