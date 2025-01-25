

namespace DownTrack.Domain.Status;

public enum EquipmentStatus
{
    Active,
    Inactive,
    UnderMaintenance
}

public static class EquipmentStatusHelper
{
    /// <summary>
    /// Validates whether the specified status belongs to any of the registered equipmentStatus
    /// </summary>
    public static bool IsValidStatus(string status) =>
        Enum.TryParse(typeof(EquipmentStatus), status, out _);
}