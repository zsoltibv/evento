using Evento.Common;

namespace Evento.Application.Bookings.DeleteBooking;

public record DeleteBookingCommand(
    int BookingId,
    string UserId,
    bool IsAdmin
) : ICommand;