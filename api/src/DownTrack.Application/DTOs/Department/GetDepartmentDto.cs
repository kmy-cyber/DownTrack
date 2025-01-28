
namespace DownTrack.Application.DTO;

// Dto para la creacion y respuesta GET de un departamento
public class GetDepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int SectionId { get; set; }
    public string SectionName {get;set;}= null!;
}

