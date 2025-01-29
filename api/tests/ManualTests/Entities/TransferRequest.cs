using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;


public static class TransferRequest
{
    private static readonly Random _random = new Random();

    public static async Task RegisterTransferRequestAsync()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5217");

        // Obtener técnicos y equipos
        var user = await Section.GetUsersAsync(client);

        var managers = user.Where(u => u.userRole == "SectionManager").ToList();
        var equipment = await Maintenance.GetEquipmentAsync(client);
        var departaments = await EquipmentReceptor.GetDepartmentsAsync(client);


        for (int i = 1; i <= 250; i++)
        {
            var manager = managers[_random.Next(managers.Count)];
            var equip = equipment[_random.Next(equipment.Count)];
            var departament = departaments[_random.Next(departaments.Count)];

            var request = new
            {
                Id = i,
                Date = DateTime.UtcNow.ToString("o"), // Fecha en formato ISO8601
                Status = "Unregistered",
                SectionManagerId = manager.id,
                EquipmentId = equip.id,
                ArrivalDepartmentId = departament.id,
                ArrivalSectionId = departament.sectionId
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/TransferRequest/POST", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully registered trasnfer request with ID: {request.Id}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} for request ID: {request.Id}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
        }
    }
}
