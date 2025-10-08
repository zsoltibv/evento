using Evento.Application.Common;
using Evento.Application.Common.Errors;
using Evento.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Evento.Application.Bookings.UpdateBooking;

public class UpdateBookingHandler(IBookingService service) : ICommandHandler<UpdateBookingCommand>
{
    public async Task<IResult> Handle(UpdateBookingCommand command)
    {
        var booking = await service.GetByIdAsync(command.Id);
        if (booking is null)
        {
            return Results.NotFound();
        }

        var statusString = command.Dto.Status;
        if (command is { IsAdmin: false } && (statusString.EqualsStatus(BookingStatus.Approved) ||
                                              statusString.EqualsStatus(BookingStatus.Rejected)))
        {
            return Results.Json(BookingErrors.UserCannotApproveOrRejectBooking,
                statusCode: StatusCodes.Status403Forbidden);
        }

        if (!command.IsAdmin && booking.UserId != command.UserId)
        {
            return Results.Forbid();
        }

        if (statusString.EqualsStatus(BookingStatus.Approved))
        {
            var overlap = await service.AnyOverlappingApprovedBookingsAsync(
                command.Id,
                booking.VenueId,
                booking.StartDate,
                booking.EndDate);

            if (overlap)
            {
                return Results.BadRequest(BookingErrors.OverlappingAnyApprovedBooking);
            }
        }

        var updated = await service.UpdateAsync(command.Id, command.Dto);
        return Results.Ok(updated);
    }
}