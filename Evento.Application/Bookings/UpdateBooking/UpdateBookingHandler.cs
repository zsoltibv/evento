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

        if (command is { IsAdmin: false, Dto.Status: BookingStatus.Approved })
        {
            return Results.Json(BookingErrors.UserCannotApproveBooking,
                statusCode: StatusCodes.Status403Forbidden);
        }

        if (!command.IsAdmin && booking.UserId != command.UserId)
        {
            return Results.Forbid();
        }

        if (command.Dto.Status == BookingStatus.Approved)
        {
            var overlap = await service.AnyOverlappingApprovedBookingsAsync(
                command.Id,
                command.Dto.VenueId!.Value,
                command.Dto.StartDate!.Value,
                command.Dto.EndDate!.Value);

            if (overlap)
            {
                return Results.Json(BookingErrors.OverlappingAnyApprovedBooking,
                    statusCode: StatusCodes.Status400BadRequest);
            }
        }

        var updated = await service.UpdateAsync(command.Id, command.Dto);
        return Results.Ok(updated);
    }
}