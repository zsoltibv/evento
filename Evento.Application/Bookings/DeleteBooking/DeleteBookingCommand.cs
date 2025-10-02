using ICommand = Evento.Domain.Common.ICommand;

namespace Evento.Application.Bookings.DeleteBooking;

public record DeleteBookingCommand(
    int BookingId,
    string UserId,
    bool IsAdmin
) : ICommand;