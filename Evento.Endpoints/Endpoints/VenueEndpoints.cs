using Evento.Application.Common;
using Evento.Application.Venues.GetVenueById;
using Evento.Application.Venues.GetVenueBySlug;
using Evento.Application.Venues.GetVenues;

namespace Evento.Endpoints.Endpoints;

public static class VenueEndpoints
{
    public static WebApplication MapVenueEndpoints(this WebApplication app)
    {
        var venuesGroup = app.MapGroup("/api/venues");

        venuesGroup.MapGet("/", async (IQueryHandler<GetVenuesQuery> handler)
                => await handler.Handle(new GetVenuesQuery()))
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        venuesGroup.MapGet("/{id:int}",
                async (int id, IQueryHandler<GetVenueByIdQuery> handler) =>
                    await handler.Handle(new GetVenueByIdQuery(id)))
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
        
        venuesGroup.MapGet("/slug/{slug}",
                async (string slug, IQueryHandler<GetVenueBySlugQuery> handler) =>
                    await handler.Handle(new GetVenueBySlugQuery(slug)))
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }
}