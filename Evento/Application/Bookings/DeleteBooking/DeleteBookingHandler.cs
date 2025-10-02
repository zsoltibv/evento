using Evento.Common;
using Evento.Services;

namespace Evento.Application.Bookings.DeleteBooking;

public class DeleteBookingHandler(IBookingService service) : ICommandHandler<DeleteBookingCommand>
{
    public async Task<IResult> Handle(DeleteBookingCommand command)
    {
        var booking = await service.GetByIdAsync(command.BookingId);
        if (booking is null)
            return Results.NotFound();

        if (!command.IsAdmin && booking.UserId != command.UserId)
            return Results.Forbid();

        await service.DeleteAsync(booking.Id);
        return Results.NoContent();
    }
}