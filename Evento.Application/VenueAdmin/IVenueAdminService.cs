using Evento.Domain.Models;

namespace Evento.Application.VenueAdmin;

public interface IVenueAdminService
{
    Task AssignVenueAdminAsync(int venueId, string userId);
    Task RemoveVenueAdminAsync(int venueId, string userId);
    Task<IEnumerable<AppUser>> GetVenueAdminsAsync(int venueId);
    Task<bool> IsVenueAdminAsync(int venueId, string userId);
}
