using Evento.Application.Common.Dto;
using Evento.Application.Services.Interfaces;
using Evento.Domain;
using Evento.Domain.Enums;
using Evento.Domain.Models;

namespace Evento.Application.Services;

public class RoleRequestService(IRoleRequestRepository repository) : IRoleRequestService
{
    public async Task<RoleRequestDto> RequestVenueAdminAsync(string userId, int venueId)
    {
        var request = new RoleRequest
        {
            UserId = userId,
            VenueId = venueId,
            RoleName = AppRoles.VenueAdmin,
            Status = RequestStatus.Pending,
            RequestDate = DateTime.UtcNow
        };

        await repository.AddAsync(request);

        return new RoleRequestDto
        {
            RoleName = request.RoleName,
            Status = request.Status,
            RequestDate = request.RequestDate,
            VenueId = request.VenueId,
        };
    }

    public async Task<bool> HasPendingRequestAsync(string userId, int venueId) =>
        await repository.ExistsPendingRequestAsync(userId, venueId, AppRoles.VenueAdmin);

    public async Task<IEnumerable<RoleRequestDto>> GetRoleRequestsAsync(string? userId = null)
    {
        var requests = string.IsNullOrEmpty(userId)
            ? await repository.GetAllAsync()
            : await repository.GetByUserIdAsync(userId);

        return requests.Select(r => new RoleRequestDto
        {
            RoleName = r.RoleName,
            Status = r.Status,
            RequestDate = r.RequestDate,
            VenueId = r.VenueId,
            Venue = r.Venue
        }).ToList();
    }
}