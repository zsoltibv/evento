using Evento.Domain.Models;

namespace Evento.Application.Venues;

public interface IVenueService
{
    Task<IEnumerable<Venue>> GetAllAsync();
    Task<Venue?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
}