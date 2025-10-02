using Evento.Common;

namespace Evento.Application.Bookings.GetBookingById;

public record GetBookingByIdQuery(
    int BookingId,
    string UserId,
    bool IsAdmin
) : IQuery;