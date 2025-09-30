using System.Security.Claims;
using Evento.Dto;
using Evento.Services;

namespace Evento.Endpoints;

public static class BookingEndpoints
{
    public static WebApplication MapBookingEndpoints(this WebApplication app)
    {
        var bookingsGroup = app.MapGroup("/api/bookings");

        bookingsGroup.MapGet("/", async (IBookingService service) =>
                Results.Ok(await service.GetAllAsync()))
            .RequireAuthorization("Admin")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapGet("/my", async (IBookingService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var bookings = await service.GetByUserAsync(userId);
                return Results.Ok(bookings);
            })
            .RequireAuthorization("User")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapGet("/{id:int}", async (int id, IBookingService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                var isAdmin = user.IsInRole("Admin");

                var booking = await service.GetByIdAsync(id);

                if (booking == null) return Results.NotFound();
                if (!isAdmin && booking.UserId != userId) return Results.Forbid();

                return Results.Ok(booking);
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapPost("/", async (CreateBookingDto createDto, IBookingService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var created = await service.CreateAsync(userId, createDto);
                return Results.Created($"/api/bookings/{created.Id}", created);
            })
            .RequireAuthorization("User")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapPut("/{id:int}",
                async (int id, UpdateBookingDto updateDto, IBookingService service, ClaimsPrincipal user) =>
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                    var isAdmin = user.IsInRole("Admin");

                    if (!await service.ExistsAsync(id)) return Results.NotFound();

                    var updated = await service.UpdateAsync(id, updateDto, userId, isAdmin);
                    return updated == null ? Results.Forbid() : Results.Ok(updated);
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapDelete("/{id:int}", async (int id, IBookingService service, ClaimsPrincipal user) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                var isAdmin = user.IsInRole("Admin");

                if (!await service.ExistsAsync(id)) return Results.NotFound();

                var deleted = await service.DeleteAsync(id, userId, isAdmin);
                return deleted ? Results.NoContent() : Results.Forbid();
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        return app;
    }
}