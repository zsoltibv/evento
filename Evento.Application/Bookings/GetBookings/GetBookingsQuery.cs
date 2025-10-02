using Evento.Domain.Common;

namespace Evento.Application.Bookings.GetBookings;

public record GetBookingsQuery(
    string UserId,
    bool IsAdmin,
    bool IsUser
) : IQuery;