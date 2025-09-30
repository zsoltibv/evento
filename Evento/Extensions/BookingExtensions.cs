using Evento.Dto;

namespace Evento.Extensions;

public static class BookingExtensions
{
    public static BookingDto ToDto(this Models.Booking booking)
        => new(
            booking.Id,
            booking.UserId,
            booking.StartDate,
            booking.EndDate,
            booking.BookingDate,
            booking.Status.ToString(),
            booking.VenueId
        );
    
    public static IEnumerable<BookingDto> ToDto(this IEnumerable<Models.Booking> bookings)
        => bookings.Select(b => b.ToDto());
}