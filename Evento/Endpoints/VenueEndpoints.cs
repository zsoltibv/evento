using Evento.Extensions;
using Evento.Services;

namespace Evento.Endpoints;

public static class VenueEndpoints
{
    public static WebApplication MapVenueEndpoints(this WebApplication app)
    {
        var venuesGroup = app.MapGroup("/api/venues");

        venuesGroup.MapGet("/", async (IVenueService service) =>
                Results.Ok((await service.GetAllAsync()).ToDto()))
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        venuesGroup.MapGet("/{id:int}", async (int id, IVenueService service) =>
            {
                var venue = await service.GetByIdAsync(id);
                return venue is not null ? Results.Ok(venue.ToDto()) : Results.NotFound();
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }
}