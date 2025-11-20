using Evento.Application.Common.Dto;
using Evento.Application.Common.Extensions;
using Evento.Application.Services.Interfaces;
using Evento.Domain;
using Evento.Domain.Enums;
using Evento.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Evento.Infrastructure.Services;

public class BookingService(IBookingRepository repo) : IBookingService
{
    public async Task<IEnumerable<BookingWithInfo>> GetAllAsync()
    {
        var bookings = await repo.GetAllAsync();
        return bookings.Select(b => b.ToDtoWithVenueName()).ToList();
    }

    public async Task<IEnumerable<BookingWithInfo>> GetByUserAsync(string userId)
    {
        var bookings = await repo.GetByUserAsync(userId);
        return bookings.Select(b => b.ToDtoWithVenueName()).ToList();
    }

    public async Task<BookingDto?> GetByIdAsync(int id)
    {
        var booking = await repo.GetByIdAsync(id);
        return booking?.ToDto();
    }
    
    public async Task<BookingDetailsDto?> GetWithDetailsByIdAsync(int id)
    {
        var booking = await repo.GetByIdAsync(id);
        return booking?.ToDetailsDto();
    }
    
    public async Task<IEnumerable<BookingWithInfo>> GetBookingsByVenueIdsAsync(string userId, IEnumerable<int> venueIds)
    {
        var bookings = await repo.GetBookingsByVenueIdsAsync(userId, venueIds);
        return bookings.Select(b => b.ToDtoWithVenueName()).ToList();
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

        if (updateDto.StartDate.HasValue)
        {
            booking.StartDate = updateDto.StartDate.Value;
        }

        if (updateDto.EndDate.HasValue)
        {
            booking.EndDate = updateDto.EndDate.Value;
        }

        if (updateDto.VenueId.HasValue)
        {
            booking.VenueId = updateDto.VenueId.Value;
        }

        if (!string.IsNullOrWhiteSpace(updateDto.Status))
        {
            booking.Status = updateDto.Status.ToBookingStatus();
        }

        var updated = await repo.UpdateAsync(booking);
        return updated.ToDto();
    }

    public async Task<bool> DeleteAsync(int id) => await repo.DeleteAsync(id);

    public async Task<bool> AnyOverlappingApprovedBookingsAsync(int id, int venueId, DateTime start,
        DateTime end)
    {
        return await repo.GetAll()
            .AnyAsync(b =>
                b.Id != id &&
                b.VenueId == venueId &&
                b.Status == BookingStatus.Approved &&
                b.StartDate < end &&
                b.EndDate > start
            );
    }

    public async Task<bool> UserHasOverlappingApprovedOrPendingBookingsAsync(string userId, int venueId, DateTime start,
        DateTime end)
    {
        return await repo.GetByUser(userId)
            .Where(b => b.VenueId == venueId &&
                        (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Approved) &&
                        b.StartDate < end &&
                        b.EndDate > start)
            .AnyAsync();
    }

    public async Task<BookingDto?> UpdatePaymentAsync(int bookingId, UpdateBookingPaymentDto paymentDto)
    {
        var booking = await repo.GetByIdAsync(bookingId);
        if (booking is null)
            return null;

        // Update only payment info
        booking.IsPaid = paymentDto.IsPaid;
        booking.AmountPaid = paymentDto.AmountPaid;

        var updated = await repo.UpdateAsync(booking);
        return updated.ToDto();
    }
}