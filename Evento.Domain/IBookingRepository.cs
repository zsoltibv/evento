using Evento.Domain.Models;

namespace Evento.Domain;

public interface IBookingRepository
{
    Task<IEnumerable<Booking>> GetAllAsync();
    Task<IEnumerable<Booking>> GetByUserAsync(string userId);
    Task<Booking?> GetByIdAsync(int id);
    Task<Booking> CreateAsync(Booking booking);
    Task<Booking> UpdateAsync(Booking booking);
    Task<bool> DeleteAsync(int id);
    IQueryable<Booking> GetAll();
    IQueryable<Booking> GetByUser(string userId);
}