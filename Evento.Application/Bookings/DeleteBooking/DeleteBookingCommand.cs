using Common_ICommand = Evento.Application.Common.ICommand;
using ICommand = Evento.Application.Common.ICommand;

namespace Evento.Application.Bookings.DeleteBooking;

public record DeleteBookingCommand(
    int BookingId,
    string UserId,
    bool IsAdmin
) : Common_ICommand;