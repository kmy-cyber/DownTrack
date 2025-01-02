using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DownTrack.Domain.Entities;

public class Section : GenericEntity
{
    public required string Name { get; set; } = "name";

    [JsonIgnore]
    public ICollection<Department> Departments{get;set;} = new List<Department>();


}