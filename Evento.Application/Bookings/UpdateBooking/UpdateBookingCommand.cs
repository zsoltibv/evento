using Evento.Application.Common.Dto;
using ICommand = Evento.Application.Common.ICommand;

namespace Evento.Application.Bookings.UpdateBooking;

public record UpdateBookingCommand(
    int Id,
    UpdateBookingDto Dto,
    string UserId,
    bool IsAdmin
) : ICommand;