using Evento.Application.Common;
using Evento.Application.Common.Dto;

namespace Evento.Application.Bookings.CreateBooking;

public record CreateBookingCommand(
    CreateBookingDto Dto,
    string UserId
) : ICommand;