
namespace DownTrack.Application.DTO;

// DTO para el registro de una evaluacion
public class EvaluationDto
{
    public int Id {get;set;}
    public int TechnicianId { get; set; }
    public int SectionManagerId { get; set; }
    public string Description { get; set; } = string.Empty;

}
