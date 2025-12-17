using Evento.Application.Common;
using Evento.Application.Common.Dto;

namespace Evento.Application.Bookings.GetBookings;

public record GetBookingsQuery(
    string UserId,
    bool IsAdmin,
    bool IsUser,
    BookingFilter Filter
) : IQuery;