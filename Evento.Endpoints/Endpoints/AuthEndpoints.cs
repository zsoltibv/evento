using Evento.Application.Auth.Login;
using Evento.Application.Auth.Register;
using Evento.Application.Common;
using Evento.Application.Common.Dto;
using Evento.Endpoints.Helpers;

namespace Evento.Endpoints.Endpoints;

public static class AuthEndpoints
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/api/auth");

        authGroup.MapPost("/register", async (RegisterDto dto, ICommandHandler<RegisterCommand> handler) =>
            {
                var command = new RegisterCommand(dto);
                return await handler.Handle(command);
            })
            .WithValidation<RegisterDto>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        authGroup.MapPost("/login", async (LoginDto dto, IQueryHandler<LoginQuery> handler) =>
            {
                var query = new LoginQuery(dto);
                return await handler.Handle(query);
            })
            .WithValidation<LoginDto>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }
}