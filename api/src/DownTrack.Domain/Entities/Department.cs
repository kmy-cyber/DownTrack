
namespace DownTrack.Domain.Entities;
/// <summary>
/// Represents a department within the system.
/// </summary>
public class Department : GenericEntity
{
    /// <summary>
    /// Gets or sets the name of the department.
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Gets or sets the ID of the section this department belongs to.
    /// </summary>
    public int SectionId { get; set; }
    /// <summary>
    /// Gets or sets the creation date of the department.
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Gets or sets the section this department belongs to.
    /// </summary
    public Section Section { get; set; } = null!;
    /// <summary>
    /// Gets or sets the collection of equipment receptors associated with this department.
    /// </summary>
    public ICollection<EquipmentReceptor> EquipmentReceptors { get; set; } = new List<EquipmentReceptor>();
    /// <summary>
    /// Gets or sets the collection of outgoing transfer requests for this department.
    /// </summary>
    public ICollection<TransferRequest> OutgoingTransferRequests { get; set; } = new List<TransferRequest>();
    /// <summary>
    /// Gets or sets the collection of incoming transfer requests for this department.
    /// </summary
    public ICollection<TransferRequest> IncomingTransferRequests { get; set; } = new List<TransferRequest>();
    /// <summary>
    /// Gets or sets the collection of equipment associated with this department.
    /// </summary>
    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();

}