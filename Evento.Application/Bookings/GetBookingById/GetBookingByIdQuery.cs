using Evento.Domain.Common;

namespace Evento.Application.Bookings.GetBookingById;

public record GetBookingByIdQuery(
    int BookingId,
    string UserId,
    bool IsAdmin
) : IQuery;