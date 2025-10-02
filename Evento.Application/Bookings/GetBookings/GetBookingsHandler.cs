using Evento.Domain.Common;
using Microsoft.AspNetCore.Http;

namespace Evento.Application.Bookings.GetBookings;

public class GetBookingsHandler(IBookingService service) : IQueryHandler<GetBookingsQuery>
{
    public async Task<IResult> Handle(GetBookingsQuery query)
    {
        if (query.IsAdmin)
        {
            var allBookings = await service.GetAllAsync();
            return Results.Ok(allBookings);
        }

        if (!query.IsUser) return Results.Forbid();

        var userBookings = await service.GetByUserAsync(query.UserId);
        return Results.Ok(userBookings);
    }
}