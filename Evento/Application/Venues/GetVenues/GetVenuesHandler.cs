using Evento.Common;
using Evento.Extensions;
using Evento.Services;

namespace Evento.Application.Venues.GetVenues;

public class GetVenuesHandler(IVenueService service) : IQueryHandler<GetVenuesQuery>
{
    public async Task<IResult> Handle(GetVenuesQuery query)
    {
        var venues = await service.GetAllAsync();
        return Results.Ok(venues.ToDto());
    }
}