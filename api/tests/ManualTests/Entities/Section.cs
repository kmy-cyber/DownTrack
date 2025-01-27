
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;

public class UserDto
{
    public int id { get; set; }
    public string name { get; set; }
    public string userRole { get; set; }
    public string email { get; set; }
    public string userName { get; set; }
}


public static class Section
{
    private static readonly Random _random = new Random();

    public static async Task<List<UserDto>> GetUsersAsync(HttpClient client)
    {
        var response = await client.GetAsync("/api/EmployeeNew/GET_ALL");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error fetching users: {response.StatusCode}");
            return new List<UserDto>();
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<UserDto>>(json) ?? new List<UserDto>();
    }

    public static async Task RegisterSectionAsync()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5217");

        var users = await GetUsersAsync(client);
        var sectionManagers = users.Where(u => u.userRole == "SectionManager").ToList();


        for (int i = 1; i <= 30; i++) // Crear secciones dinámicamente
        {
            var manager = sectionManagers[_random.Next(sectionManagers.Count)];

            var section = new
            {
                Id = i,
                Name = $"Section_{i}",
                SectionManagerId = manager.id
            };

            var content = new StringContent(
                JsonSerializer.Serialize(section),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/SectionNew/POST", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully registered section with ID: {section.Id} and Manager: {manager.id}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} for section ID: {section.Id}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
        }
    }

}



