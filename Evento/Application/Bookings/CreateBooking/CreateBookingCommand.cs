using Evento.Common;
using Evento.Dto;

namespace Evento.Application.Bookings.CreateBooking;

public record CreateBookingCommand(
    CreateBookingDto Dto,
    string UserId
) : ICommand;