using Evento.Dto;
using Evento.Extensions;
using Evento.Models;
using Evento.Repository;

namespace Evento.Services;

public class BookingService(IBookingRepository repo) : IBookingService
{
    public async Task<List<BookingDto>> GetAllAsync()
    {
        var bookings = await repo.GetAllAsync();
        return bookings.Select(b => b.ToDto()).ToList();
    }

    public async Task<List<BookingDto>> GetByUserAsync(string userId)
    {
        var bookings = await repo.GetByUserAsync(userId);
        return bookings.Select(b => b.ToDto()).ToList();
    }

    public async Task<BookingDto?> GetByIdAsync(int id)
    {
        var booking = await repo.GetByIdAsync(id);
        return booking?.ToDto();
    }

    public async Task<BookingDto> CreateAsync(string userId, CreateBookingDto createDto)
    {
        var booking = new Booking
        {
            UserId = userId,
            StartDate = createDto.StartDate,
            EndDate = createDto.EndDate,
            VenueId = createDto.VenueId,
            BookingDate = DateTime.UtcNow,
            Status = BookingStatus.Pending
        };

        var created = await repo.CreateAsync(booking);
        return created.ToDto();
    }

    public async Task<BookingDto?> UpdateAsync(int id, UpdateBookingDto updateDto, string userId, bool isAdmin)
    {
        var booking = await repo.GetByIdAsync(id);
        if (booking == null) return null;

        if (!isAdmin && booking.UserId != userId) return null;

        booking.StartDate = updateDto.StartDate;
        booking.EndDate = updateDto.EndDate;
        booking.VenueId = updateDto.VenueId;
        booking.Status = updateDto.Status;

        var updated = await repo.UpdateAsync(id, booking);
        return updated?.ToDto();
    }

    public async Task<bool> DeleteAsync(int id, string userId, bool isAdmin)
    {
        var booking = await repo.GetByIdAsync(id);
        if (booking == null) return false;

        if (!isAdmin && booking.UserId != userId) return false;

        return await repo.DeleteAsync(id);
    }
    
    public async Task<bool> ExistsAsync(int id)
    {
        var booking = await repo.GetByIdAsync(id);
        return booking != null;
    }
}