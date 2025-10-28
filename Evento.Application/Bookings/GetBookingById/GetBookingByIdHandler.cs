using Evento.Application.Common;
using Evento.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Evento.Application.Bookings.GetBookingById;

public class GetBookingByIdHandler(IBookingService service) : IQueryHandler<GetBookingByIdQuery>
{
    public async Task<IResult> Handle(GetBookingByIdQuery query)
    {
        var booking = await service.GetWithDetailsByIdAsync(query.BookingId);
        if (booking is null)
            return Results.NotFound();

        if (query.IsAdmin || booking.UserId == query.UserId)
            return Results.Ok(booking);

        return Results.Forbid();
    }
}