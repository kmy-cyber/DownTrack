using Manual;

var apiClient = new ApiClient();

// await apiClient.CreateEquipmentAsync();

await apiClient.RegisterUserAsync();

await apiClient.RegisterSectionAsync();
await apiClient.RegisterDepartmentAsync();

// await apiClient.Equipment()


// await apiClient.DeleteEquipmentAsync();
