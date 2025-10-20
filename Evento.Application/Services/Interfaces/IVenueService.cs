using Evento.Domain.Models;

namespace Evento.Application.Services.Interfaces;

public interface IVenueService
{
    Task<IEnumerable<Venue>> GetAllAsync();
    Task<Venue?> GetByIdAsync(int id);
    Task<Venue?> GetBySlugAsync(string slug);
    Task<bool> ExistsAsync(int id);
    Task<string> GenerateUniqueSlug(string name);
}