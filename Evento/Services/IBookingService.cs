using Evento.Dto;

namespace Evento.Services;

public interface IBookingService
{
    Task<List<BookingDto>> GetAllAsync();
    Task<List<BookingDto>> GetByUserAsync(string userId);
    Task<BookingDto?> GetByIdAsync(int id);
    Task<BookingDto> CreateAsync(string userId, CreateBookingDto createDto);
    Task<BookingDto?> UpdateAsync(int id, UpdateBookingDto updateDto);
    Task<bool> DeleteAsync(int id);
}