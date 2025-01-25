
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;
public class ApiClient
{
    private static readonly Random _random = new Random();
    private static readonly string[] _names = { "Router", "Switch", "Firewall", "Server", "Access Point" };
    private static readonly string[] _types = { "Networking", "Security", "Infrastructure", "Wireless" };
    private static readonly string[] _statuses = { "Operational", "Under Maintenance", "Out of Service" };

    public async Task DeleteEquipmentAsync()
    {
        using var client = new HttpClient();


        // Define la base URL de la API
        client.BaseAddress = new Uri("http://localhost:5217/api/Equipment/");

        for (int i = 1; i <= 10000; i++)
        {

            var response = await client.DeleteAsync($"{i}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully deleted equipment with ID: {i}");
            }
            else
            {
                Console.WriteLine(response);
                Console.WriteLine($"Error: {response.StatusCode}");
                break;
            }
        }
    }
    public async Task CreateEquipmentAsync()
    {
        using var client = new HttpClient();


        client.BaseAddress = new Uri("http://localhost:5217/api/Equipment/");

        for (int i = 1; i <= 10000; i++)
        {
            var equipment = new
            {
                Id = i, // id consecutivo
                Name = _names[_random.Next(_names.Length)], // Nombre aleatorio
                Type = _types[_random.Next(_types.Length)], // Tipo aleatorio
                Status = _statuses[_random.Next(_statuses.Length)], // Estado aleatorio
                DateOfAdquisition = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") // Fecha actual
            };

            // Serializa el objeto a JSON
            var content = new StringContent(
                JsonSerializer.Serialize(equipment),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("POST", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully created equipment with ID: {equipment.Id}");
            }
            else
            {
                Console.WriteLine(response);
                Console.WriteLine($"Error: {response.StatusCode} for equipment ID: {equipment.Id}");
                break;
            }
        }
    }

    private static readonly string[] _roles = { "Technician", "SectionManager", "Director", "Administrator" };
    private static readonly string[] _specialties = { "Mechanic", "Electrician", "Programmer" };
    private static readonly string[] _emails = { "example1@gmail.com", "example2@gmail.com", "example3@gmail.com" };
    //private static readonly Random _random = new Random();

    // Lista para almacenar los usuarios con rol SectionManager
    private static readonly List<int> _sectionManagers = new List<int>();

    public async Task RegisterUserAsync()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5217/api/Authentication/register");

        for (int i = 151; i <= 170; i++)
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

                // Si el usuario tiene el rol "SectionManager", guárdalo en la lista
                if (userRole == "SectionManager")
                {
                    _sectionManagers.Add(user.Id);
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} for user ID: {user.Id}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
        }
    }


    private static readonly List<(int Id,string Name)> _section = new List<(int Id, string Name)>();

    public async Task RegisterSectionAsync()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5217/api/Section/POST");

        for (int i = 342; i <= 361; i++) // Aseguramos una sección por cada SectionManager
        {
            var sectionManager = _sectionManagers[_random.Next(_sectionManagers.Count)];
            Console.WriteLine(sectionManager); 
            var section = new
            {
                Id = i,
                Name = $"Section_{i}",
                SectionManagerId = sectionManager
            };

            // Serializa el objeto a JSON
            var content = new StringContent(
                JsonSerializer.Serialize(section),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(string.Empty, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully registered section with ID: {section.Id} and Manager: {sectionManager}");
                _section.Add((section.Id,section.Name));
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} for section ID: {section.Id}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
        }
    }



     public async Task RegisterDepartmentAsync()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5217/api/Department/POST");

        for (int i = 1223; i <= 1250; i++) // Aseguramos una sección por cada SectionManager
        {
            var section = _section[_random.Next(_section.Count)]; 
            Console.WriteLine(section.Name);
            Console.WriteLine(section.Id);
            var department = new
            {
                Id = i,
                Name = $"Department_{i}",
                SectionId = section.Id,
                SectionName = section.Name
            };

            // Serializa el objeto a JSON
            var content = new StringContent(
                JsonSerializer.Serialize(department),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(string.Empty, content);

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
