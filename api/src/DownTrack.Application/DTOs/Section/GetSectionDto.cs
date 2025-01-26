
namespace DownTrack.Application.DTO;

// DTO para la creacion de unas Section
public class GetSectionDto
{
    public int Id { get; set; }
    public int SectionManagerId {get;set;}
    public string SectionManagerUserName {get;set;} = null!;
    public string Name { get; set; } = null!;
    
}

