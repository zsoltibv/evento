using Evento.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Evento.Endpoints.Helpers;

public static class MigrationService
{
    public static void InitMigrations(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EventoDbContext>();
        db.Database.Migrate();
    }
}