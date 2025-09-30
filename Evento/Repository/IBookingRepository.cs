using Evento.Models;

namespace Evento.Repository;

public interface IBookingRepository
{
    Task<List<Booking>> GetAllAsync();
    Task<List<Booking>> GetByUserAsync(string userId);
    Task<Booking?> GetByIdAsync(int id);
    Task<Booking> CreateAsync(Booking booking);
    Task<Booking?> UpdateAsync(int id, Booking booking);
    Task<bool> DeleteAsync(int id);
}