using Evento.Application.Common.Dto;

namespace Evento.Application.Services.Interfaces;

public interface IBookingService
{
    Task<IEnumerable<BookingWithInfo>> GetAllAsync();
    Task<IEnumerable<BookingWithInfo>> GetByUserAsync(string userId);
    Task<BookingDto?> GetByIdAsync(int id);
    Task<BookingDetailsDto?> GetWithDetailsByIdAsync(int id);
    Task<IEnumerable<BookingWithInfo>> GetBookingsByVenueIdsAsync(string userId, IEnumerable<int> venueIds);
    Task<BookingDto> CreateAsync(string userId, CreateBookingDto createDto);
    Task<BookingDto?> UpdateAsync(int id, UpdateBookingDto updateDto);
    Task<bool> DeleteAsync(int id);
    Task<bool> AnyOverlappingApprovedBookingsAsync(int id, int venueId, DateTime start, DateTime end);
    Task<bool> UserHasOverlappingApprovedOrPendingBookingsAsync(string userId, int venueId, DateTime start, DateTime end);
}