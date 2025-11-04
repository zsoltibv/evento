using System.Security.Claims;
using Evento.Application.Services.Interfaces;
using Evento.Endpoints.Helpers;

namespace Evento.Endpoints.Endpoints;

public static class ChatEndpoints
{
    public static WebApplication MapChatEndpoints(this WebApplication app)
    {
        var chatsGroup = app.MapGroup("/api/chats");

        chatsGroup.MapGet("/user", async (
                ClaimsPrincipal user,
                IChatService chatService
            ) =>
            {
                var chats = await chatService.GetUserChatsAsync(user.GetUserId());
                return Results.Ok(chats);
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        
        return app;
    }
}