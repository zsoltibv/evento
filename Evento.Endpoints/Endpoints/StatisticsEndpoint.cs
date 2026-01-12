using System.Security.Claims;
using Evento.Application.Common.Dto;
using Evento.Application.Services.Interfaces;
using Evento.Endpoints.Helpers;

namespace Evento.Endpoints.Endpoints;

public static class StatisticsEndpoints
{
    public static WebApplication MapStatisticsEndpoints(this WebApplication app)
    {
        var statisticsGroup = app.MapGroup("/api/statistics");

        statisticsGroup.MapGet("/", async (
            IBookingService bookingService,
            ClaimsPrincipal user,
            int? month
        ) =>
        {
            var userId = user.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            var bookings = await bookingService.GetByUserAsync(userId);

            if (month.HasValue)
            {
                bookings = bookings.Where(b => b.StartDate.Month == month.Value);
            }

            var bookingsList = bookings.ToList();

            var bookingsCount = bookingsList.Count;

            var totalRevenue = bookingsList
                .Where(b => b.IsPaid)
                .Sum(b => b.AmountPaid);
            
            var weeklyRevenue = bookingsList
                .GroupBy(b => (b.StartDate.Day - 1) / 7)
                .OrderBy(g => g.Key)
                .Select(g => g.Sum(b => b.AmountPaid))
                .ToArray();

            var dto = new StatisticsDto(
                BookingsCount: bookingsCount,
                VenuesCount: bookingsList.Select(b => b.VenueId).Distinct().Count(),
                TotalRevenue: totalRevenue,
                WeeklyRevenue: weeklyRevenue.Length == 0
                    ? [0, 0, 0, 0]
                    : weeklyRevenue
            );

            return Results.Ok(dto);
        })
        .RequireAuthorization();

        return app;
    }
}