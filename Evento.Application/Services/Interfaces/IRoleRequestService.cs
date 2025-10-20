using Evento.Application.Common.Dto;

namespace Evento.Application.Services.Interfaces;

public interface IRoleRequestService
{
    Task<bool> HasPendingRequestAsync(string userId, int venueId);
    Task<RoleRequestDto> RequestVenueAdminAsync(string userId, int venueId);
    Task<IEnumerable<RoleRequestDto>> GetRoleRequestsAsync(string? userId = null);
}