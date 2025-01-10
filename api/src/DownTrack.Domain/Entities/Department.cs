
using System.ComponentModel.DataAnnotations;

namespace DownTrack.Domain.Entities;

public class Department : GenericEntity
{
    public string Name { get; set; } = null!;
    [Key]
    public int SectionId { get; set; }

    // todo dpto tiene obligado una Seccion
    public Section Section { get; set; } = null!;
    public ICollection<TransferRequest> TransferRequests { get; set; } = new List<TransferRequest>();
}