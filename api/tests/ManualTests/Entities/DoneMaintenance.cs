using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;

public class PagedResultDto<T>
{
    public IEnumerable<T>? items { get; set; }
    public int totalCount { get; set; }
    public int pageNumber { get; set; }
    public int pageSize { get; set; }
    public string? nextPageUrl { get; set; }
    public string? previousPageUrl { get; set; }
}


public class EquipmentDto
{
    public int id { get; set; }
    public string name { get; set; } = null!;
    public string type { get; set; } = null!;
    public string status { get; set; } = null!;
    public DateTime dateOfadquisition { get; set; } = DateTime.Now;
    public int departmentId { get; set; }
    public int sectionId { get; set; }
    public string departmentName { get; set; } = null!;
    public string sectionName { get; set; } = null!;
}

public static class Maintenance
{
    private static readonly Random _random = new Random();

    public static async Task<List<EquipmentDto>> GetEquipmentAsync(HttpClient client)
    {
        var response = await client.GetAsync("/api/Employee/GetPaged/?pageNumber=1&pageSize=1000");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error fetching equipment: {response.StatusCode}");
            return new List<EquipmentDto>();
        }

        var json = await response.Content.ReadAsStringAsync();

        // Deserializar el objeto completo y extraer solo los elementos de "Items"
        var pagedResult = JsonSerializer.Deserialize<PagedResultDto<EquipmentDto>>(json);

        return pagedResult?.items?.ToList() ?? new List<EquipmentDto>();
    }


    public static async Task RegisterMaintenanceAsync()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5217");

        // Obtener técnicos y equipos
        var user = await Section.GetUsersAsync(client);

        var technicians = user.Where(u => u.userRole == "Technician").ToList();
        var equipment = await GetEquipmentAsync(client);

        for (int i = 1; i <= 200; i++)
        {
            var technician = technicians[_random.Next(technicians.Count)];
            var equip = equipment[_random.Next(equipment.Count)];

            var maintenance = new
            {
                Id = i,
                TechnicianId = technician.id,
                Type = "Preventive", // O "Corrective" según sea necesario
                EquipmentId = equip.id,
                Date = DateTime.UtcNow.ToString("o"), // Fecha en formato ISO8601
                Cost = _random.Next(100, 1000) // Costo aleatorio entre 100 y 1000
            };

            var content = new StringContent(
                JsonSerializer.Serialize(maintenance),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/DoneMaintenance/POST", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully registered maintenance with ID: {maintenance.Id}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} for maintenance ID: {maintenance.Id}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
        }
    }
}
