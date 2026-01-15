using System.Security.Claims;
using Evento.Application.Common.Dto;
using Evento.Application.Services.Interfaces;
using Evento.Domain.Enums;
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
                int? month,
                int? year
            ) =>
            {
                var userId = user.GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Results.Unauthorized();

                var targetYear = year ?? DateTime.UtcNow.Year;

                var dto = user.IsAdmin()
                    ? await BuildAdminStatistics(bookingService, month, targetYear)
                    : await BuildUserStatistics(bookingService, userId, month, targetYear);

                return Results.Ok(dto);
            })
            .RequireAuthorization();

        static async Task<StatisticsDto> BuildUserStatistics(
            IBookingService bookingService,
            string userId,
            int? month,
            int year
        )
        {
            var bookings = await bookingService.GetByUserAsync(userId);

            var filtered = FilterBookings(bookings, month, year);

            var bookingsList = filtered.ToList();

            return new StatisticsDto(
                BookingsCount: bookingsList.Count,
                VenuesCount: bookingsList.Select(b => b.VenueId).Distinct().Count(),
                TotalRevenue: bookingsList
                    .Where(b => b.IsPaid && b.Status == BookingStatus.Approved.ToString())
                    .Sum(b => b.AmountPaid),
                WeeklyRevenue: CalculateWeeklyRevenue(bookingsList),
                VenueStatistics: null
            );
        }

        static async Task<StatisticsDto> BuildAdminStatistics(
            IBookingService bookingService,
            int? month,
            int year
        )
        {
            var bookings = await bookingService.GetAllAsync();

            var filtered = FilterBookings(bookings, month, year);

            var bookingsList = filtered.ToList();

            var venueStats = bookingsList
                .GroupBy(b => new { b.VenueId, b.VenueName })
                .Select(g => new VenueStatisticDto(
                    VenueId: g.Key.VenueId,
                    VenueName: g.Key.VenueName,
                    BookingsCount: g.Count(),
                    TotalRevenue: g
                        .Where(b => b.IsPaid && b.Status == BookingStatus.Approved.ToString())
                        .Sum(b => b.AmountPaid)
                ))
                .OrderByDescending(v => v.BookingsCount)
                .ToList();

            return new StatisticsDto(
                BookingsCount: bookingsList.Count,
                VenuesCount: bookingsList.Select(b => b.VenueId).Distinct().Count(),
                TotalRevenue: bookingsList
                    .Where(b => b.IsPaid && b.Status == BookingStatus.Approved.ToString())
                    .Sum(b => b.AmountPaid),
                WeeklyRevenue: CalculateWeeklyRevenue(bookingsList),
                VenueStatistics: venueStats
            );
        }

        static IEnumerable<BookingWithInfo> FilterBookings(
            IEnumerable<BookingWithInfo> bookings,
            int? month,
            int year
        )
        {
            bookings = bookings.Where(b => b.StartDate.Year == year);

            if (month.HasValue)
                bookings = bookings.Where(b => b.StartDate.Month == month.Value);

            return bookings;
        }

        static decimal[] CalculateWeeklyRevenue(
            List<BookingWithInfo> bookings
        )
        {
            var weekly = bookings
                .Where(b => b.IsPaid && b.Status == BookingStatus.Approved.ToString())
                .GroupBy(b => (b.StartDate.Day - 1) / 7)
                .OrderBy(g => g.Key)
                .Select(g => g.Sum(b => b.AmountPaid))
                .ToArray();

            return weekly.Length == 0
                ? [0m, 0m, 0m, 0m]
                : weekly;
        }

        return app;
    }
}