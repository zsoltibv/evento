using Evento.Application.Common.Dto;

namespace Evento.Application.Services.Interfaces;

public interface IBookingService
{
    Task<IEnumerable<BookingWithVenueNameDto>> GetAllAsync();
    Task<IEnumerable<BookingWithVenueNameDto>> GetByUserAsync(string userId);
    Task<BookingDto?> GetByIdAsync(int id);
    Task<BookingDto> CreateAsync(string userId, CreateBookingDto createDto);
    Task<BookingDto?> UpdateAsync(int id, UpdateBookingDto updateDto);
    Task<bool> DeleteAsync(int id);
    Task<bool> AnyOverlappingApprovedBookingsAsync(int id, int venueId, DateTime start, DateTime end);
    Task<bool> UserHasOverlappingApprovedOrPendingBookingsAsync(string userId, int venueId, DateTime start, DateTime end);
}