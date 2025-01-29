using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;


public class TransferRequestDto
{
    public int id { get; set; }
    public DateTime date { get; set; }
    public string status {get;set;}
    public int sectionManagerId { get; set; }
    public string sectionManagerUserName { get; set; } = null!;
    public int equipmentId { get; set; }
    public int arrivalDepartmentId { get; set; }
    public int arrivalSectionId { get; set; }
    public string arrivalDepartmentName { get; set; } = null!;
    public string arrivalSectionName { get; set; } = null!;

}

public class EquipmentReceptorDto
{

    public int id { get; set; }
    public string name { get; set; }
    public int departmentId { get; set; }
    public int sectionId { get; set; }
    public string departmentName { get; set; }
    public string sectionName { get; set; }
    public string userName { get; set; }

}


public static class Transfer
{
    private static readonly Random _random = new Random();

    public static async Task<List<EquipmentReceptorDto>> GetReceptorAsync(HttpClient client)
    {
        var response = await client.GetAsync("/api/EquipmentReceptor/GetPaged/?pageNumber=1&pageSize=1000");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error fetching trasnfer request: {response.StatusCode}");
            return new List<EquipmentReceptorDto>();
        }

        var json = await response.Content.ReadAsStringAsync();

        // Deserializar el objeto completo y extraer solo los elementos de "Items"
        var pagedResult = JsonSerializer.Deserialize<PagedResultDto<EquipmentReceptorDto>>(json);

        return pagedResult?.items?.ToList() ?? new List<EquipmentReceptorDto>();
    }
    public static async Task<List<TransferRequestDto>> GetRequestsAsync(HttpClient client)
    {
        var response = await client.GetAsync("/api/TransferRequest/GetPaged/?pageNumber=1&pageSize=1000");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error fetching trasnfer request: {response.StatusCode}");
            return new List<TransferRequestDto>();
        }

        var json = await response.Content.ReadAsStringAsync();

        // Deserializar el objeto completo y extraer solo los elementos de "Items"
        var pagedResult = JsonSerializer.Deserialize<PagedResultDto<TransferRequestDto>>(json);

        return pagedResult?.items?.ToList() ?? new List<TransferRequestDto>();
    }

public static async Task RegisterTransferAsync()
{
    using var client = new HttpClient();
    client.BaseAddress = new Uri("http://localhost:5217");

    // Obtener técnicos y equipos
    var user = await Section.GetUsersAsync(client);
    var receptors = await GetReceptorAsync(client);

    var supervisors = user.Where(u => u.userRole == "ShippingSupervisor").ToList();
    var requests = await GetRequestsAsync(client);

    for (int i = 63; i <= 200; i++)
    {
        if (requests.Count == 0)
        {
            Console.WriteLine("No hay más solicitudes disponibles.");
            break;
        }

        var supervisor = supervisors[_random.Next(supervisors.Count)];
        var request = requests[_random.Next(requests.Count)];
        var receptor = receptors.FirstOrDefault(r => r.departmentId == request.arrivalDepartmentId);

        if (receptor == null)
        {
            continue;
        }
        requests.Remove(request);

        try
        {
            var transfer = new
            {
                Id = i,
                RequestId = request.id,
                ShippingSupervisorId = supervisor.id,
                EquipmentReceptorId = receptor.id,
                Date = DateTime.UtcNow.ToString("o"), // Fecha en formato ISO8601
            };

            var content = new StringContent(
                JsonSerializer.Serialize(transfer),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/Transfer/POST", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully registered transfer with ID: {transfer.Id}");

                // **Actualizar el estado de la TransferRequest**
                TransferRequestDto updateStatus = new TransferRequestDto
                {
                    id = request.id,
                    date = request.date,                    
                    status = "Registered",
                    sectionManagerId = request.sectionManagerId,
                    equipmentId = request.equipmentId,
                    arrivalDepartmentId = request.arrivalDepartmentId,
                    arrivalSectionId = request.arrivalSectionId,
                    arrivalDepartmentName = request.arrivalDepartmentName,
                    arrivalSectionName = request.arrivalSectionName
                };
                /*
                    public int id { get; set; }
    public DateTime date { get; set; }
    public int sectionManagerId { get; set; }
    public string sectionManagerUserName { get; set; } = null!;
    public int equipmentId { get; set; }
    public int arrivalDepartmentId { get; set; }
    public int arrivalSectionId { get; set; }
    public string arrivalDepartmentName { get; set; } = null!;
    public string arrivalSectionName { get; set; } = null!;

                */

                var statusContent = new StringContent(
                    JsonSerializer.Serialize(updateStatus),
                    Encoding.UTF8,
                    "application/json"
                );
                
                var statusResponse = await client.PutAsync("/api/TransferRequest/PUT", statusContent);

                if (statusResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Updated TransferRequest ID {request.id} to 'Registered'");
                }
                else
                {
                    Console.WriteLine($"Failed to update TransferRequest ID {request.id}: {statusResponse.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            continue;
        }
    }
}
}
