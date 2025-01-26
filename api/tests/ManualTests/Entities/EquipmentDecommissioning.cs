using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;


public static class EquipmentDecommissioning
{
    private static readonly Random _random = new Random();

    public static async Task RegisterEquipmentDecomissioningAsync()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5217");

        // Obtener técnicos y equipos
        var user = await Section.GetUsersAsync(client);

        var technicians = user.Where(u => u.userRole == "Technician").ToList();
        var equipment = await Maintenance.GetEquipmentAsync(client);
        var receptors = user.Where(u => u.userRole == "EquipmentReceptor").ToList();

        for (int i = 1; i <= 100; i++)
        {
            var technician = technicians[_random.Next(technicians.Count)];
            var equip = equipment[_random.Next(equipment.Count)];
            var receptor = receptors[_random.Next(receptors.Count)];

            var baja = new
            {
                Id = i,
                TechnicianId = technician.id,
                EquipmentId = equip.id,
                ReceptorId = receptor.id,
                Date = DateTime.UtcNow.ToString("o"), // Fecha en formato ISO8601
                Cause = "break",
                Status = "Pending"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(baja),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/EquipmentDecommissioning/POST", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully registered baja with ID: {baja.Id}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} for baja ID: {baja.Id}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
        }
    }
}
