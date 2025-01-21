// using Manual;

// var apiClient = new ApiClient();

// // await apiClient.CreateEquipmentAsync();

// await apiClient.RegisterUserAsync();

// // await apiClient.DeleteEquipmentAsync();

public System.Linq.Expression;


// Agregar algunos valores a la lista
Lista.list.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

// Consultar números mayores a 5
var result = Lista.Query(x => x > 5);

// Mostrar los resultados
Console.WriteLine("Números mayores que 5:");
foreach (var num in result)
{
    Console.WriteLine(num);
}

public static class Lista
{
    public static List<int> list = new List<int>();

    public static IQueryable<int> Query(Expression<Func<int, bool>> exp)
    {

        return list.AsQueryable().Where(exp);

    }

}