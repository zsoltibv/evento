using Evento.Application.Common;
using Evento.Application.Common.Extensions;
using Evento.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Evento.Application.Venues.GetVenueBySlug;

public class GetVenueBySlugHandler(IVenueService service) : IQueryHandler<GetVenueBySlugQuery>
{
    public async Task<IResult> Handle(GetVenueBySlugQuery query)
    {
        var venue = await service.GetBySlugAsync(query.Slug);
        return venue is not null
            ? Results.Ok(venue.ToDtoWithBookings())
            : Results.NotFound();
    }
}