using System.Security.Claims;
using Evento.Application;
using Evento.Application.Bookings.CreateBooking;
using Evento.Application.Bookings.DeleteBooking;
using Evento.Application.Bookings.GetBookingById;
using Evento.Application.Bookings.GetBookings;
using Evento.Application.Bookings.UpdateBooking;
using Evento.Application.Common.Dto;
using Evento.Domain.Common;
using Evento.Infrastructure.Helpers;

namespace Evento.Endpoints.Endpoints;

public static class BookingEndpoints
{
    public static WebApplication MapBookingEndpoints(this WebApplication app)
    {
        var bookingsGroup = app.MapGroup("/api/bookings");

        bookingsGroup.MapGet("/", async (
                IQueryHandler<GetBookingsQuery> handler,
                ClaimsPrincipal user
            ) =>
            {
                var query = new GetBookingsQuery(
                    user.GetUserId(),
                    user.IsAdmin(),
                    user.IsUser()
                );
                
                return await handler.Handle(query);
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapGet("/{id:int}", async (
                int id,
                IQueryHandler<GetBookingByIdQuery> handler,
                ClaimsPrincipal user
            ) =>
            {
                var query = new GetBookingByIdQuery(
                    BookingId: id,
                    UserId: user.GetUserId(),
                    IsAdmin: user.IsAdmin()
                );

                return await handler.Handle(query);
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapPost("/", async (
                CreateBookingDto dto,
                ICommandHandler<CreateBookingCommand> handler,
                ClaimsPrincipal user
            ) =>
            {
                var command = new CreateBookingCommand(dto, user.GetUserId());
                return await handler.Handle(command);
            })
            .WithValidation<CreateBookingDto>()
            .RequireAuthorization(AppRoles.User)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapPut("/{id:int}", async (
                int id,
                UpdateBookingDto dto,
                ICommandHandler<UpdateBookingCommand> handler,
                ClaimsPrincipal user) =>
            {
                var command = new UpdateBookingCommand(
                    id,
                    dto,
                    user.GetUserId(),
                    user.IsAdmin()
                );

                return await handler.Handle(command);
            })
            .WithValidation<UpdateBookingDto>()
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        bookingsGroup.MapDelete("/{id:int}", async (
                int id,
                ICommandHandler<DeleteBookingCommand> handler,
                ClaimsPrincipal user
            ) =>
            {
                var command = new DeleteBookingCommand(
                    id,
                    user.GetUserId(),
                    user.IsAdmin()
                );

                return await handler.Handle(command);
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        return app;
    }
}