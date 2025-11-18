using Evento.Application.Services.Interfaces;

namespace Evento.Endpoints.Endpoints;

public static class UserEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/users");

        userGroup.MapGet("/customer-id/{userId}", async (string userId, IUserService userService) =>
            {
                var customerId = await userService.GetCustomerId(userId);
                return Results.Ok(customerId);
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}