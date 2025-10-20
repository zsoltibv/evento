using Evento.Domain.Models;

namespace Evento.Domain;

public interface IRoleRequestRepository
{
    Task<bool> ExistsPendingRequestAsync(string userId, int venueId, string roleName);
    Task AddAsync(RoleRequest roleRequest);
    Task<IEnumerable<RoleRequest>> GetAllAsync();
    Task<IEnumerable<RoleRequest>> GetByUserIdAsync(string userId);
}