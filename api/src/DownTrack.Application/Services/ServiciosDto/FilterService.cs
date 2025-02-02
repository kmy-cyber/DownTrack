
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using DownTrack.Application.Interfaces;


namespace DownTrack.Application.Services.Specials;

public class FilterService<TEntity> : IFilterService<TEntity>
{
    public IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> query, Dictionary<string, object>? filters)
    {
        if (filters == null || filters.Count == 0)
            return query;  // Si no hay filtros, devolver la consulta sin modificaciones

        ParameterExpression param = Expression.Parameter(typeof(TEntity), "entity");
        Expression? filterExpression = null;

        foreach (var filter in filters)
        {
            string propertyName = filter.Key;
            object? filterValue = filter.Value;

            // Obtener la propiedad de la entidad
            PropertyInfo? property = typeof(TEntity).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null) continue; // Si no existe la propiedad, ignorarla

            // Obtener la expresión de la propiedad de la entidad
            Expression left = Expression.Property(param, property);

            // Convertir el valor del filtro al tipo de la propiedad
            object convertedValue;
            try
            {
                convertedValue = ConvertFilterValue(filterValue, property.PropertyType);
            }
            catch
            {
                continue; // Si falla la conversión, ignorar este filtro
            }

            Expression right = Expression.Constant(convertedValue, property.PropertyType);

            // Crear la condición según el tipo de la propiedad
            Expression condition;
            if (property.PropertyType == typeof(string))
            {
                // Si es string, usar Contains para búsqueda parcial
                MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                condition = Expression.Call(left, containsMethod, right);
            }
            else
            {
                // Para otros tipos, usar igualdad
                condition = Expression.Equal(left, right);
            }

            // Combinar las condiciones con "AND"
            filterExpression = filterExpression == null ? condition : Expression.AndAlso(filterExpression, condition);
        }

        if (filterExpression != null)
        {
            // Convertir la expresión en una lambda y aplicarla a la consulta
            var lambda = Expression.Lambda<Func<TEntity, bool>>(filterExpression, param);
            query = query.Where(lambda);
        }

        return query;
    }

    private static object ConvertFilterValue(object value, Type targetType)
    {
        if (value is JsonElement jsonElement)
        {
            if (targetType == typeof(string) && jsonElement.ValueKind == JsonValueKind.String)
                return jsonElement.GetString()!;

            if (targetType == typeof(int) && jsonElement.ValueKind == JsonValueKind.Number)
                return jsonElement.GetInt32();

            if (targetType == typeof(double) && jsonElement.ValueKind == JsonValueKind.Number)
                return jsonElement.GetDouble();

            if (targetType == typeof(bool) && (jsonElement.ValueKind == JsonValueKind.True || jsonElement.ValueKind == JsonValueKind.False))
                return jsonElement.GetBoolean();

            if (targetType == typeof(DateTime) && jsonElement.ValueKind == JsonValueKind.String)
                return DateTime.Parse(jsonElement.GetString()!);
        }

        return Convert.ChangeType(value, targetType);
    }


}