using Evento.Application.Venues;
using Evento.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Evento.Infrastructure.Services;

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
            .Include(v => v.Bookings)
            .ThenInclude(b => b.Venue)
            .AsNoTracking()
            .Where(v => v.Id == id)
            .Select(v => new Venue
            {
                Id = v.Id,
                Name = v.Name,
                Description = v.Description,
                ImageUrl = v.ImageUrl,
                Location = v.Location,
                Capacity = v.Capacity,
                Slug = v.Slug,
                Bookings = v.Bookings
                    .OrderByDescending(b => b.BookingDate)
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Venue?> GetBySlugAsync(string slug)
    {
        return await db.Venues
            .Include(v => v.Bookings)
            .ThenInclude(b => b.Venue)
            .AsNoTracking()
            .Where(v => v.Slug == slug)
            .Select(v => new Venue
            {
                Id = v.Id,
                Name = v.Name,
                Description = v.Description,
                ImageUrl = v.ImageUrl,
                Location = v.Location,
                Capacity = v.Capacity,
                Slug = v.Slug,
                Bookings = v.Bookings
                    .OrderByDescending(b => b.BookingDate)
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await db.Venues.AnyAsync(v => v.Id == id);
    }
    
    public async Task<string> GenerateUniqueSlug(string name)
    {
        var slug = name.ToLowerInvariant()
            .Trim()
            .Replace(" ", "-")
            .Replace(".", "")
            .Replace(",", "")
            .Replace(":", "")
            .Replace(";", "")
            .Replace("/", "")
            .Replace("\\", "");
        
        var uniqueSlug = slug;
        var counter = 1;

        while (await db.Venues.AnyAsync(v => v.Slug == uniqueSlug))
        {
            uniqueSlug = $"{slug}-{counter++}";
        }

        return uniqueSlug;
    }
}