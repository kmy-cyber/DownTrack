
namespace DownTrack.Application.DTO;
public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int SectionId { get; set; }

}


public class DepartmentPresentationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int SectionId { get; set; }
    public string SectionName {get; set;}

}

