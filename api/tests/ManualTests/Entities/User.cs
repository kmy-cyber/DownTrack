
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;

public static class User
{
    private static readonly Random _random = new Random();
    
    private static readonly string[] _roles = { "Technician", "SectionManager", "Director", "Administrator", "ShippingSupervisor" };
    private static readonly string[] _specialties = { "Mechanic", "Electrician", "Programmer" };
    private static readonly string[] _emails = { "example1@gmail.com", "example2@gmail.com", "example3@gmail.com" };
 
    public static async Task RegisterUserAsync()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5217/api/Authentication/register");

        for (int i = 341; i <= 500; i++)
        {
            var userRole = _roles[_random.Next(_roles.Length)]; 

            var user = new
            {
                Id = i,
                Name = $"User_{i}", 
                UserName = $"username_{i}", 
                Email = _emails[_random.Next(_emails.Length)], 
                Password = $"Password_{i}!", 
                UserRole = userRole,
                Specialty = _specialties[_random.Next(_specialties.Length)], 
                Salary = _random.Next(30000, 100000), 
                ExpYears = _random.Next(1, 20), 
                DepartamentId = _random.Next(1, 10), 
                SectionId = _random.Next(1, 5)
            };

            // Serializa el objeto a JSON
            var content = new StringContent(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(string.Empty, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully registered user with ID: {user.Id}");

                
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} for user ID: {user.Id}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
        }
    }


    


}
