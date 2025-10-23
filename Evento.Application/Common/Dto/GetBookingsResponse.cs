namespace Evento.Application.Common.Dto;

public class GetBookingsResponse
{
    public IEnumerable<BookingWithInfo> UserBookings { get; set; } = [];
    public IEnumerable<BookingWithInfo> VenueBookings { get; set; } = [];
}