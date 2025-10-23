namespace Evento.Application.Services.Interfaces;

public interface IVenueAdminService
{
    Task AssignVenueAdminAsync(int venueId, string userId);
    Task SendVenueAdminApprovedEmailAsync(string email, string venueName);
    Task<int[]> GetVenueIdsByUserIdAsync(string userId);
    Task<bool> IsUserVenueAdminAsync(string userId, int venueId);
}