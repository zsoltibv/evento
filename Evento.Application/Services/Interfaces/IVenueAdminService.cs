namespace Evento.Application.Services.Interfaces;

public interface IVenueAdminService
{
    Task AssignVenueAdminAsync(int venueId, string userId);
}