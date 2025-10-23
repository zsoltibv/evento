using Evento.Application.Common.Dto;
using Evento.Domain.Enums;
using Evento.Domain.Models;

namespace Evento.Application.Services.Interfaces;

public interface IRoleRequestService
{
    Task<bool> HasActiveRequestAsync(string userId, int venueId);
    Task<RoleRequestDto> RequestVenueAdminAsync(string userId, int venueId);
    Task<IEnumerable<RoleRequestDto>> GetRoleRequestsAsync(string? userId = null);
    Task<RoleRequest?> GetRoleRequestByIdAsync(int id);
    Task UpdateStatusAsync(int id, RequestStatus newStatus);
}