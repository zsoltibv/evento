using Evento.Models;
using Microsoft.EntityFrameworkCore;

namespace Evento.Services;

public class VenueService(EventoDbContext db) : IVenueService
{
    public async Task<IEnumerable<Venue>> GetAllAsync()
    {
        return await db.Venues
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Venue?> GetByIdAsync(int id)
    {
        return await db.Venues
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id);
    }
    
    public async Task<bool> ExistsAsync(int id)
    {
        return await db.Venues.AnyAsync(v => v.Id == id);
    }
}