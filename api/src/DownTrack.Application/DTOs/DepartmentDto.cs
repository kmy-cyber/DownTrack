using DownTrack.Domain.Entities;
namespace DownTrack.Application.DTO
{
    public class DepartmentDto
{
    public int Id {get;set;}
    public required string Name { get; set; } = "name";
    public int SectionId {get; set;}

}
}
