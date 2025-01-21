
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual
{
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
            client.BaseAddress = new Uri("/Equipment/");

            for (int i = 1; i <= 10000 ; i++)
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

            client.BaseAddress = new Uri("/Equipment/");

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
    }
}
