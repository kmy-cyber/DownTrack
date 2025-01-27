using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;


public static class Evaluation
{
    private static readonly Random _random = new Random();

    public static async Task RegisterEvaluationAsync()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5217");

        // Obtener técnicos y jefes de sección

        var users = await Section.GetUsersAsync(client);

        var sectionManagers = users.Where(u => u.userRole == "SectionManager").ToList();
         var technicians = users.Where(u => u.userRole == "Technician").ToList();

        if (technicians.Count == 0 || sectionManagers.Count == 0)
        {
            Console.WriteLine("Technicians or Section Managers are not available.");
            return;
        }

        for (int i = 1; i <= 50; i++) // Crear 50 evaluaciones
        {
            // Seleccionar técnico y jefe de sección aleatoriamente
            var technician = technicians[_random.Next(technicians.Count)];
            var sectionManager = sectionManagers[_random.Next(sectionManagers.Count)];

            // Crear el objeto evaluación
            var evaluation = new
            {
                Id = i,
                TechnicianId = technician.id,
                SectionManagerId = sectionManager.id,
                Description = $"Evaluation for Technician {technician.name} by Manager {sectionManager.name}"
            };

            // Serializar el objeto a JSON
            var content = new StringContent(
                JsonSerializer.Serialize(evaluation),
                Encoding.UTF8,
                "application/json"
            );

            // Enviar la solicitud POST
            var response = await client.PostAsync("/api/Evaluation/POST", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully registered evaluation with ID: {evaluation.Id}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} for evaluation ID: {evaluation.Id}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
        }
    }
}
