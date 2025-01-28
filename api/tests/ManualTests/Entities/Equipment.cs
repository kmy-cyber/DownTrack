using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;

public class DepartmentDto
{
    public int id { get; set; }
    public string name { get; set; }
    public int sectionId { get; set; }
    public string sectionName { get; set; }
}

public static class Equipment
{
    private static readonly Random _random = new Random();

    private static async Task<List<DepartmentDto>> GetDepartmentsAsync(HttpClient client)
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

    public static async Task RegisterEquipmentAsync()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5217");

        // Obtener la lista de departamentos
        var departments = await GetDepartmentsAsync(client);

        if (departments.Count == 0)
        {
            Console.WriteLine("No departments available to assign equipment.");
            return;
        }

        for (int i = 1; i <= 500; i++) // Crear 500 equipos
        {
            // Seleccionar un departamento aleatorio
            var department = departments[_random.Next(departments.Count)];

            // Crear el objeto equipo
            var equipment = new
            {
                Id = i,
                Name = $"Equipment_{i}", // Nombre único
                Type = $"Type_{_random.Next(1, 10)}", // Tipo aleatorio (por ejemplo, Type_1, Type_2, ...)
                Status = "Active", // Estado fijo
                DateOfAdquisition = DateTime.UtcNow, // Fecha actual UTC
                DepartmentId = department.id,
                SectionId = department.sectionId
            };

            // Serializar el objeto a JSON
            var content = new StringContent(
                JsonSerializer.Serialize(equipment),
                Encoding.UTF8,
                "application/json"
            );

            // Enviar la solicitud POST
            var response = await client.PostAsync("/api/Equipment/POST", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully registered equipment with Name: {equipment.Name}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} for equipment ID: {equipment.Id}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
        }
    }
}
