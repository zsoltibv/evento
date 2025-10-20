using Evento.Domain;
using Evento.Domain.Enums;
using Evento.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Evento.Infrastructure.Repository;

public class RoleRequestRepository(EventoDbContext db) : IRoleRequestRepository
{
    public async Task<bool> ExistsPendingRequestAsync(string userId, int venueId, string roleName)
    {
        return await db.RoleRequests.AnyAsync(r =>
            r.UserId == userId &&
            r.VenueId == venueId &&
            r.RoleName == roleName &&
            r.Status == RequestStatus.Pending);
    }

    public async Task AddAsync(RoleRequest roleRequest)
    {
        db.RoleRequests.Add(roleRequest);
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<RoleRequest>> GetAllAsync()
    {
        return await db.RoleRequests
            .Include(r => r.Venue)
            .OrderByDescending(r => r.RequestDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<RoleRequest>> GetByUserIdAsync(string userId)
    {
        return await db.RoleRequests
            .Where(r => r.UserId == userId)
            .Include(r => r.Venue)
            .OrderByDescending(r => r.RequestDate)
            .ToListAsync();
    }
}