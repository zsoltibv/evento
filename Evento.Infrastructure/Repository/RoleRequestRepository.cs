using Evento.Domain;
using Evento.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Evento.Infrastructure.Repository;

public class RoleRequestRepository(EventoDbContext db) : IRoleRequestRepository
{
    public async Task<bool> HasActiveRequestAsync(string userId, int venueId, string roleName)
    {
        return await db.RoleRequests.AnyAsync(r =>
            r.UserId == userId &&
            r.VenueId == venueId &&
            r.RoleName == roleName);
    }

    public async Task AddAsync(RoleRequest roleRequest)
    {
        db.RoleRequests.Add(roleRequest);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(RoleRequest roleRequest)
    {
        db.RoleRequests.Update(roleRequest);
        await db.SaveChangesAsync();
    }

    public async Task<RoleRequest?> GetByIdAsync(int id)
    {
        return await db.RoleRequests
            .Include(u => u.User)
            .Include(v => v.Venue)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<RoleRequest>> GetAllAsync()
    {
        return await db.RoleRequests
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Venue)
            .OrderByDescending(r => r.RequestDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<RoleRequest>> GetByUserIdAsync(string userId)
    {
        return await db.RoleRequests
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .Include(r => r.User)
            .Include(r => r.Venue)
            .OrderByDescending(r => r.RequestDate)
            .ToListAsync();
    }
}