
// using System;
// using System.Net.Http;
// using System.Text;
// using System.Text.Json;
// using System.Threading.Tasks;

// namespace Manual;
// public class ApiClient
// {
//     private static readonly Random _random = new Random();
//     private static readonly string[] _names = { "Router", "Switch", "Firewall", "Server", "Access Point" };
//     private static readonly string[] _types = { "Networking", "Security", "Infrastructure", "Wireless" };
//     private static readonly string[] _statuses = { "Operational", "Under Maintenance", "Out of Service" };

//     public async Task DeleteEquipmentAsync()
//     {
//         using var client = new HttpClient();

//         // Define la base URL de la API
//         client.BaseAddress = new Uri("http://localhost:5217/api/Equipment/");

//         for (int i = 1; i <= 10000; i++)
//         {

//             var response = await client.DeleteAsync($"{i}");

//             if (response.IsSuccessStatusCode)
//             {
//                 Console.WriteLine($"Successfully deleted equipment with ID: {i}");
//             }
//             else
//             {
//                 Console.WriteLine(response);
//                 Console.WriteLine($"Error: {response.StatusCode}");
//                 break;
//             }
//         }
//     }
//     public async Task CreateEquipmentAsync()
//     {
//         using var client = new HttpClient();

//         client.BaseAddress = new Uri("http://localhost:5217/api/Equipment/");

//         for (int i = 1; i <= 10000; i++)
//         {
//             var equipment = new
//             {
//                 Id = i, // id consecutivo
//                 Name = _names[_random.Next(_names.Length)], // Nombre aleatorio
//                 Type = _types[_random.Next(_types.Length)], // Tipo aleatorio
//                 Status = _statuses[_random.Next(_statuses.Length)], // Estado aleatorio
//                 DateOfAdquisition = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") // Fecha actual
//             };

//             // Serializa el objeto a JSON
//             var content = new StringContent(
//                 JsonSerializer.Serialize(equipment),
//                 Encoding.UTF8,
//                 "application/json"
//             );

//             var response = await client.PostAsync("POST", content);

//             if (response.IsSuccessStatusCode)
//             {
//                 Console.WriteLine($"Successfully created equipment with ID: {equipment.Id}");
//             }
//             else
//             {
//                 Console.WriteLine(response);
//                 Console.WriteLine($"Error: {response.StatusCode} for equipment ID: {equipment.Id}");
//                 break;
//             }
//         }
//     }


//     private static readonly string[] _roles = { "Technician" };
//     private static readonly string[] _specialties = { "Mechanic", "Electrician", "Programmer" };
//     private static readonly string[] _emails = { "example1@gmail.com", "example2@gmail.com", "example3@gmail.com" };



//     public async Task RegisterUserAsync()
//     {

//         using var client = new HttpClient();

//         client.BaseAddress = new Uri("http://localhost:5217/api/Authentication/register");

//         for (int i = 11; i <= 100; i++)
//         {
//             var user = new
//             {
//                 Id = i,
//                 Name = $"User_{i}", // Nombre dinámico
//                 UserName = $"username_{i}", // Nombre de usuario dinámico
//                 Email = _emails[_random.Next(_emails.Length)], // Email aleatorio
//                 Password = $"Password_{i}!", // Contraseña dinámica
//                 UserRole = _roles[_random.Next(_roles.Length)], // Rol aleatorio
//                 Specialty = _specialties[_random.Next(_specialties.Length)], // Especialidad aleatoria
//                 Salary = _random.Next(30000, 100000), // Salario aleatorio
//                 ExpYears = _random.Next(1, 20), // Años de experiencia aleatorios
//                 DepartamentId = _random.Next(1, 10), // ID de departamento aleatorio
//                 SectionId = _random.Next(1, 5) // ID de sección aleatorio
//             };

//             // Serializa el objeto a JSON
//             var content = new StringContent(
//                 JsonSerializer.Serialize(user),
//                 Encoding.UTF8,
//                 "application/json"
//             );

//             var response = await client.PostAsync(string.Empty, content);

//             if (response.IsSuccessStatusCode)
//             {
//                 Console.WriteLine($"Successfully registered user with ID: {user.Id}");
//             }
//             else
//             {
//                 Console.WriteLine($"Error: {response.StatusCode} for user ID: {user.Id}");
//                 Console.WriteLine(await response.Content.ReadAsStringAsync());
//                 break;
//             }

//         }
//     }
// }
