using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;


public static class EquipmentReceptor
{
    private static readonly Random _random = new Random();

    public static async Task<List<DepartmentDto>> GetDepartmentsAsync(HttpClient client)
    {
        var response = await client.GetAsync("/api/Department/GET_ALL");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error fetching departments: {response.StatusCode}");
            return new List<DepartmentDto>();
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<DepartmentDto>>(json) ?? new List<DepartmentDto>();
    }

    public static async Task RegisterEquipmentReceptorAsync()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5217");

        // Obtener la lista de departamentos
        var departments = await GetDepartmentsAsync(client);

        if (departments.Count == 0)
        {
            Console.WriteLine("No departments available to assign receptors.");
            return;
        }

        for (int i = 301; i <= 330; i++) // Crear 100 receptores
        {
            // Seleccionar un departamento aleatorio
            var department = departments[_random.Next(departments.Count)];

            // Crear el objeto receptor
            var receptor = new
            {
                Id = i,
                Name = $"Receptor_{i}",
                UserName = $"receptor{i}",
                Email = $"receptor{i}@example.com",
                Password = "Password_123!", // Contraseña genérica
                UserRole = "EquipmentReceptor",
                Specialty = $"Specialty_{_random.Next(1, 5)}", // Especialidad aleatoria (por ejemplo, Specialty_1, Specialty_2, ...)
                Salary = _random.Next(30000, 60000), // Salario entre 30,000 y 60,000
                ExpYears = _random.Next(1, 20), // Años de experiencia entre 1 y 20
                DepartmentId = department.id,
                SectionId = department.sectionId
            };

            // Serializar el objeto a JSON
            var content = new StringContent(
                JsonSerializer.Serialize(receptor),
                Encoding.UTF8,
                "application/json"
            );

            // Enviar la solicitud POST
            var response = await client.PostAsync("/api/Authentication/register", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully registered equipment receptor with Name: {receptor.Name}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} for receptor ID: {receptor.Id}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
        }
    }
}
