using Evento.Application.Common.Dto;
using Evento.Domain.Common;

namespace Evento.Application.Bookings.CreateBooking;

public record CreateBookingCommand(
    CreateBookingDto Dto,
    string UserId
) : ICommand;