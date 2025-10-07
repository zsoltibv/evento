using Evento.Application.Common;
using Evento.Application.Common.Errors;
using Microsoft.AspNetCore.Http;

namespace Evento.Application.Bookings.CreateBooking;

public class CreateBookingHandler(IBookingService service) : ICommandHandler<CreateBookingCommand>
{
    public async Task<IResult> Handle(CreateBookingCommand command)
    {
        var hasOverlap = await service.UserHasOverlappingApprovedOrPendingBookingsAsync(
            command.UserId,
            command.Dto.VenueId!.Value,
            command.Dto.StartDate!.Value,
            command.Dto.EndDate!.Value
        );

        if (hasOverlap)
        {
            return Results.BadRequest(BookingErrors.OverlappingUserApprovedOrPendingBooking);
        }

        var created = await service.CreateAsync(command.UserId, command.Dto);
        return Results.Created($"/api/bookings/{created.Id}", created);
    }
}