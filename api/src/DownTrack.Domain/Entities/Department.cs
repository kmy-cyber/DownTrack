using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace DownTrack.Domain.Entities;

public class Department : GenericEntity
{
    public required string Name { get; set; } = "name";

    public required int SectionId {get; set;}
    [JsonIgnore]
    public Section? Section { get; set; }
}