using Evento.Application.Common.Extensions;
using Evento.Domain.Common;
using Microsoft.AspNetCore.Http;

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