
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DownTrack.Infrastructure;

/// <summary>
/// Responsible for initializing the database by applying migrations automatically.
/// </summary>
public static class DatabaseInitializer
{
    /// <summary>
    /// Applies database migrations to ensure the database schema is up-to-date with the application's data model.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve required services, such as the DbContext.</param>
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {

        // Create a new scope to resolve services with a scoped lifecycle.
        using var scope = serviceProvider.CreateScope();

        // Retrieve the database context from the scoped service provider.
        var context = scope.ServiceProvider.GetRequiredService<DownTrackContext>();

        try
        {
            // Automatically applies pending migrations to the database.
            await context.Database.MigrateAsync();

        }
        catch (Exception ex)
        {
            //logger.LogError(ex, "Error al inicializar la base de datos");
            throw;
        }
    }

}
