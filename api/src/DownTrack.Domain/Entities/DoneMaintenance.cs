
namespace DownTrack.Domain.Entities;
/// <summary>
/// Represents a maintenance record for an equipment item.
/// This class inherits from GenericEntity and contains properties for
/// technician identification, maintenance type, associated equipment, date,
/// cost, and completion status.
/// </summary>
public class DoneMaintenance : GenericEntity
{
    /// <summary>
    /// Gets or sets the identifier of the technician who performed the maintenance.
    /// </summary>
    public int? TechnicianId { get; set; }
    /// <summary>
    /// Gets or sets the type of maintenance performed.
    /// </summary>
    public string Type { get; set; } = null!;
    /// <summary>
    /// Gets or sets the identifier of the equipment associated with this maintenance record.
    /// </summary>
    public int? EquipmentId { get; set; } 
    /// <summary>
    /// Gets or sets the date when the maintenance was performed.
    /// </summary>   
    public DateTime Date { get; set; } 
    /// <summary>
    /// Gets or sets the total cost of the maintenance.
    /// </summary>
    public double Cost { get; set; }
    /// <summary>
    /// Gets or sets the associated Equipment entity.
    /// </summary>
    public Equipment? Equipment { get; set; }
    /// <summary>
    /// Gets or sets the associated Technician entity.
    /// </summary>
    public Technician? Technician { get; set; }
    /// <summary>
    /// Gets or sets a flag indicating whether the maintenance is complete.
    /// </summary>
    public bool Finish {get;set;}
    
}