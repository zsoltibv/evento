using Evento.Models;

namespace Evento.Services;

public interface IVenueService
{
    Task<IEnumerable<Venue>> GetAllAsync();
    Task<Venue?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
}