using Evento.Application.Services.Interfaces;
using Evento.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Evento.Infrastructure.Services;

public class VenueAdminService(EventoDbContext context) : IVenueAdminService
{
    public async Task AssignVenueAdminAsync(int venueId, string userId)
    {
        var venue = await context.Venues
            .Include(v => v.VenueAdmins)
            .FirstOrDefaultAsync(v => v.Id == venueId);

        if (venue == null)
            throw new KeyNotFoundException("Venue not found.");

        var user = await context.Users.FindAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");
        
        if (venue.VenueAdmins.Any(a => a.Id == userId))
            return;

        venue.VenueAdmins.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task RemoveVenueAdminAsync(int venueId, string userId)
    {
        var venue = await context.Venues
            .Include(v => v.VenueAdmins)
            .FirstOrDefaultAsync(v => v.Id == venueId);

        if (venue == null)
            throw new KeyNotFoundException("Venue not found.");

        var admin = venue.VenueAdmins.FirstOrDefault(a => a.Id == userId);
        if (admin == null)
            return;

        venue.VenueAdmins.Remove(admin);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AppUser>> GetVenueAdminsAsync(int venueId)
    {
        var venue = await context.Venues
            .Include(v => v.VenueAdmins)
            .FirstOrDefaultAsync(v => v.Id == venueId);

        return venue == null ? throw new KeyNotFoundException("Venue not found.") : venue.VenueAdmins;
    }

    public async Task<bool> IsVenueAdminAsync(int venueId, string userId)
    {
        return await context.VenueAdmins
            .AnyAsync(va => va.VenueId == venueId && va.UserId == userId);
    }
}