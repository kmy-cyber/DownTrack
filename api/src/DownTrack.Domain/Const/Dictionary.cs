
using DownTrack.Domain.Entities;
using DownTrack.Domain.Roles;

namespace DownTrack.Domain.Const;

public static class GetEntity
{
    public static Type? GetEntityTypeForRole(string role)
    {
        
        var roleMappings = new Dictionary<string, Type>
    {
        { UserRole.Technician.ToString(), typeof(Technician) },
        { UserRole.EquipmentReceptor.ToString(), typeof(EquipmentReceptor) },
        {"Employee", typeof(Employee)}
    };

        return roleMappings.ContainsKey(role) ? roleMappings[role] : null;
    }
}
