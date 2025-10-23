using Evento.Application.Common.Dto;
using Evento.Domain.Models;

namespace Evento.Application.Common.Extensions;

public static class BookingExtensions
{
    public static BookingDto ToDto(this Booking booking)
        => new(
            booking.Id,
            booking.UserId,
            booking.StartDate,
            booking.EndDate,
            booking.BookingDate,
            booking.Status.ToString(),
            booking.VenueId
        );
    
    public static BookingWithInfo ToDtoWithVenueName(this Booking booking)
        => new(
            booking.Id,
            booking.UserId,
            booking.StartDate,
            booking.EndDate,
            booking.BookingDate,
            booking.Status.ToString(),
            booking.VenueId,
            booking.Venue.Name
        );
    
    public static IEnumerable<BookingDto> ToDto(this IEnumerable<Booking> bookings)
        => bookings.Select(b => b.ToDto());
    
    public static IEnumerable<BookingWithInfo> ToDtoWithVenueName(this IEnumerable<Booking> bookings)
        => bookings.Select(b => b.ToDtoWithVenueName());
}