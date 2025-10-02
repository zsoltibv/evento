using Evento.Common;
using Evento.Dto;

namespace Evento.Application.Bookings.UpdateBooking;

public record UpdateBookingCommand(
    int Id,
    UpdateBookingDto Dto,
    string UserId,
    bool IsAdmin
) : ICommand;