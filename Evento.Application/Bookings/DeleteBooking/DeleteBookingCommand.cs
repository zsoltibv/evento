using ICommand = Evento.Application.Common.ICommand;

namespace Evento.Application.Bookings.DeleteBooking;

public record DeleteBookingCommand(
    int BookingId,
    string UserId,
    bool IsAdmin
) : ICommand;