using Evento.Common;
using Evento.Extensions;
using Evento.Services;

namespace Evento.Application.Venues.GetVenueById;

public class GetVenueByIdHandler(IVenueService service) : IQueryHandler<GetVenueByIdQuery>
{
    public async Task<IResult> Handle(GetVenueByIdQuery query)
    {
        var venue = await service.GetByIdAsync(query.VenueId);
        return venue is not null
            ? Results.Ok(venue.ToDtoWithBookings())
            : Results.NotFound();
    }
}