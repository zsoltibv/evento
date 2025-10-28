using Evento.Application.Common.Dto;
using Evento.Application.Common.Extensions;
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
            Status = request.Status.ToString(),
            RequestDate = request.RequestDate,
            VenueId = request.VenueId,
        };
    }

    public async Task<bool> HasActiveRequestAsync(string userId, int venueId) =>
        await repository.HasActiveRequestAsync(userId, venueId, AppRoles.VenueAdmin);

    public async Task<IEnumerable<RoleRequestDto>> GetRoleRequestsAsync(string? userId = null)
    {
        var requests = string.IsNullOrEmpty(userId)
            ? await repository.GetAllAsync()
            : await repository.GetByUserIdAsync(userId);

        return requests.Select(r => new RoleRequestDto
        {
            Id = r.Id,
            RoleName = r.RoleName,
            Status = r.Status.ToString(),
            RequestDate = r.RequestDate,
            VenueId = r.VenueId,
            Venue = r.Venue!.ToDto(),
            User = r.User.ToDto()
        }).ToList();
    }

    public async Task<RoleRequest?> GetRoleRequestByIdAsync(int id) => await repository.GetByIdAsync(id);

    public async Task UpdateStatusAsync(int id, RequestStatus newStatus)
    {
        var request = await repository.GetByIdAsync(id);
        if (request == null)
        {
            throw new KeyNotFoundException("Role request not found.");
        }

        request.Status = newStatus;
        await repository.UpdateAsync(request);
    }
}