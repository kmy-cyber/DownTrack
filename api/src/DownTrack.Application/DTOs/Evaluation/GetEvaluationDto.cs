
namespace DownTrack.Application.DTO;

// DTO para el registro de una evaluacion
public class GetEvaluationDto
{
    public int Id {get;set;}
    public int TechnicianId { get; set; }
    public string TechnicianUserName {get;set;} = null!;
    public int SectionManagerId { get; set; }
    public string SectionManagerUserName {get;set;}= null!;
    public string Description { get; set; } = string.Empty;

}