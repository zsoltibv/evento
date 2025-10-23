using Evento.Application.Common;
using Evento.Application.Common.Dto;
using Evento.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Evento.Application.Bookings.GetBookings;

public class GetBookingsHandler(IBookingService bookingService, IVenueAdminService venueAdminService)
    : IQueryHandler<GetBookingsQuery>
{
    public async Task<IResult> Handle(GetBookingsQuery query)
    {
        if (query.IsAdmin)
        {
            var allBookings = await bookingService.GetAllAsync();
            return Results.Ok(new GetBookingsResponse
            {
                UserBookings = new List<BookingWithInfo>(),
                VenueBookings = allBookings
            });
        }

        if (!query.IsUser) return Results.Forbid();

        var userBookings = await bookingService.GetByUserAsync(query.UserId);
        var venueIds = await venueAdminService.GetVenueIdsByUserIdAsync(query.UserId);

        var venueBookings = venueIds.Length != 0
            ? await bookingService.GetBookingsByVenueIdsAsync(query.UserId,  venueIds)
            : new List<BookingWithInfo>();

        var response = new GetBookingsResponse
        {
            UserBookings = userBookings,
            VenueBookings = venueBookings
        };

        return Results.Ok(response);
    }
}