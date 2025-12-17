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
        IEnumerable<BookingWithInfo> userBookings = [];
        IEnumerable<BookingWithInfo> venueBookings = [];

        if (query.IsAdmin)
        {
            venueBookings = await bookingService.GetAllAsync();
        }
        else
        {
            if (!query.IsUser)
                return Results.Forbid();

            userBookings = await bookingService.GetByUserAsync(query.UserId);

            var venueIds = await venueAdminService.GetVenueIdsByUserIdAsync(query.UserId);

            if (venueIds.Length != 0)
            {
                venueBookings = await bookingService
                    .GetBookingsByVenueIdsAsync(query.UserId, venueIds);
            }
        }
        
        userBookings = ApplyFilter(userBookings, query.Filter);
        venueBookings = ApplyFilter(venueBookings, query.Filter);

        return Results.Ok(new GetBookingsResponse
        {
            UserBookings = userBookings.ToList(),
            VenueBookings = venueBookings.ToList()
        });
    }
    
    private static IEnumerable<BookingWithInfo> ApplyFilter(
        IEnumerable<BookingWithInfo> bookings,
        BookingFilter filter)
    {
        if (filter is null)
            return bookings;

        if (filter.FromDate is not null)
            bookings = bookings.Where(b => b.StartDate >= filter.FromDate);

        if (filter.ToDate is not null)
            bookings = bookings.Where(b => b.EndDate <= filter.ToDate);

        if (!string.IsNullOrWhiteSpace(filter.Status))
            bookings = bookings.Where(b => b.Status == filter.Status);

        if (filter.VenueId is not null)
            bookings = bookings.Where(b => b.VenueId == filter.VenueId);

        if (filter.IsPaid is not null)
            bookings = bookings.Where(b => b.IsPaid == filter.IsPaid);

        return bookings;
    }
}