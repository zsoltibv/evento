using System.Security.Claims;
using Evento.Dto;
using Evento.Extensions;
using Evento.Services;

namespace Evento.Endpoints;

public static class BookingEndpoints
{
    public static WebApplication MapBookingEndpoints(this WebApplication app)
    {
        var bookingsGroup = app.MapGroup("/api/bookings");

        bookingsGroup.MapGet("/", async (IBookingService service, ClaimsPrincipal user) =>
            {
                var userId = user.GetUserId();

                return user switch
                {
                    _ when user.IsAdmin() => Results.Ok(await service.GetAllAsync()),
                    _ when user.IsUser() => Results.Ok(await service.GetByUserAsync(userId)),
                    _ => Results.Forbid()
                };
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapGet("/{id:int}", async (int id, IBookingService service, ClaimsPrincipal user) =>
            {
                var userId = user.GetUserId();
                var booking = await service.GetByIdAsync(id);

                return booking switch
                {
                    null => Results.NotFound(),
                    _ when user.IsAdmin() => Results.Ok(booking),
                    _ when booking.UserId == userId => Results.Ok(booking),
                    _ => Results.Forbid()
                };
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapPost("/", async (CreateBookingDto createDto, IBookingService service, ClaimsPrincipal user) =>
            {
                var userId = user.GetUserId();
                var created = await service.CreateAsync(userId, createDto);

                return Results.Created($"/api/bookings/{created.Id}", created);
            })
            .WithValidation<CreateBookingDto>()
            .RequireAuthorization(AppRoles.User)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapPut("/{id:int}",
                async (int id, UpdateBookingDto updateDto, IBookingService service, ClaimsPrincipal user) =>
                {
                    var userId = user.GetUserId();
                    var booking = await service.GetByIdAsync(id);

                    return booking switch
                    {
                        null => Results.NotFound(),
                        _ when !user.IsAdmin() && booking.UserId != userId => Results.Forbid(),
                        _ => Results.Ok(await service.UpdateAsync(id, updateDto))
                    };
                })
            .WithValidation<UpdateBookingDto>()
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapDelete("/{id:int}", async (int id, IBookingService service, ClaimsPrincipal user) =>
            {
                var userId  = user.GetUserId();
                var booking = await service.GetByIdAsync(id);

                if (booking is null)
                {
                    return Results.NotFound();
                }

                if (!user.IsAdmin() && booking.UserId != userId)
                {
                    return Results.Forbid();
                }

                await service.DeleteAsync(booking.Id);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        return app;
    }
}