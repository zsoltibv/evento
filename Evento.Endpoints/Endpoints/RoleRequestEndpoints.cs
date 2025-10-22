using System.Security.Claims;
using Evento.Application;
using Evento.Application.Common;
using Evento.Application.Venues.ApproveVenueAdminCommand;
using Evento.Application.Venues.GetVenueRoles;
using Evento.Endpoints.Helpers;

namespace Evento.Endpoints.Endpoints;

public static class RoleRequestEndpoints
{
    public static WebApplication MapRoleRequestEndpoints(this WebApplication app)
    {
        var roleRequestsGroup = app.MapGroup("/api/role-requests");

        roleRequestsGroup.MapGet("/",
                async (IQueryHandler<GetVenueRolesQuery> handler, ClaimsPrincipal user) =>
                    await handler.Handle(new GetVenueRolesQuery(user.GetUserId(), user.IsAdmin())))
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        
        roleRequestsGroup.MapPost("/{id:int}/approve",
                async (int id, ICommandHandler<ApproveVenueAdminCommand> handler) =>
                {
                    var token = await handler.Handle(new ApproveVenueAdminCommand(id));
                    return Results.Ok(new { Token = token });
                })
            .RequireAuthorization(AppRoles.Admin)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}