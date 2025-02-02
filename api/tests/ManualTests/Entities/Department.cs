
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;

public class SectionDto
{
  public int id{get;set;}
  public int sectionManagerId{get;set;}
  public string sectionManagerUserName {get;set;}
  public string name {get;set;}

}
public static class Department
{
    private static readonly Random _random = new Random();

     private static async Task<List<SectionDto>> GetSectionAsync(HttpClient client)
    {
        var response = await client.GetAsync("/api/Section/GET_ALL");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error fetching users: {response.StatusCode}");
            return new List<SectionDto>();
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<SectionDto>>(json) ?? new List<SectionDto>();
    }

     public static async Task RegisterDepartmentAsync()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5217");

         var _section = await GetSectionAsync(client);


        List<string> _dates = new List<string> (){
            "2025-02-01","2025-01-01","2024-12-01","2024-11-01","2024-10-01","2024-09-01","2024-08-01","2024-07-01"
        };

        for (int i = 111; i <= 190; i++) // Aseguramos una sección por cada SectionManager
        {
            var section = _section[_random.Next(_section.Count)]; 
            var date = _dates[_random.Next(_dates.Count)]; 

            var department = new
            {
                Id = i,
                Name = $"Department_{i}",
                SectionId = section.id,
                SectionName = section.name,
                CreatedDate = date
            };

            // Serializa el objeto a JSON
            var content = new StringContent(
                JsonSerializer.Serialize(department),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/Department/POST", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully registered department with Name : {department.Name} ");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} for department ID: {department.Id}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
        }
    }
}
