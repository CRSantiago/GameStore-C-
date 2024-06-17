namespace GameStore.Data;
using Microsoft.EntityFrameworkCore;

public static class DataExtensions
{
    public static void MigrateDB(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        var dbContex = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContex.Database.Migrate();
    }
}
