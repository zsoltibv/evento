using Evento.Application.Common;
using Evento.Application.Common.Extensions;
using Evento.Application.Venues.GetVenueById;
using Microsoft.AspNetCore.Http;

namespace Evento.Application.Venues.GetVenueBySlug;

public class GetVenueBySlugHandler(IVenueService service) : IQueryHandler<GetVenueBySlugQuery>
{
    public async Task<IResult> Handle(GetVenueByIdQuery query)
    {
        var venue = await service.GetByIdAsync(query.VenueId);
        return venue is not null
            ? Results.Ok(venue.ToDtoWithBookings())
            : Results.NotFound();
    }
}