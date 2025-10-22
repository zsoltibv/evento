using Evento.Application.Services.Interfaces;
using Evento.Domain.Models;

namespace Evento.Infrastructure.Services;

public class VenueAdminService(EventoDbContext db) : IVenueAdminService
{
    public async Task AssignVenueAdminAsync(int venueId, string userId)
    {
        if ( db.VenueAdmins.Any(a => a.UserId == userId &&  a.VenueId == venueId))
        {
            return;
        }

        db.VenueAdmins.Add(new VenueAdmin
        {
            VenueId = venueId,
            UserId = userId
        });
        await db.SaveChangesAsync();
    }
}