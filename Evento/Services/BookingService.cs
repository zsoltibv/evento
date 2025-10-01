using Evento.Dto;
using Evento.Enums;
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
            StartDate = createDto.StartDate!.Value,
            EndDate = createDto.EndDate!.Value,
            VenueId = createDto.VenueId!.Value,
            BookingDate = DateTime.UtcNow,
            Status = BookingStatus.Pending
        };

        var created = await repo.CreateAsync(booking);
        return created.ToDto();
    }

    public async Task<BookingDto?> UpdateAsync(int id, UpdateBookingDto updateDto)
    {
        var booking = await repo.GetByIdAsync(id);
        if (booking is null)
        {
            return null;
        }

        booking.StartDate = updateDto.StartDate!.Value;
        booking.EndDate = updateDto.EndDate!.Value;
        booking.VenueId = updateDto.VenueId!.Value;
        booking.Status = updateDto.Status!.Value;

        var updated = await repo.UpdateAsync(booking);
        return updated.ToDto();
    }

    public async Task<bool> DeleteAsync(int id) => await repo.DeleteAsync(id);
}