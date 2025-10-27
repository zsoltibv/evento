using System.Security.Claims;
using Evento.Application;

namespace Evento.Endpoints.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new InvalidOperationException("User ID claim not found.");
    
    public static string GetUserName(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.GivenName)?.Value
        ?? throw new InvalidOperationException("User name claim not found.");
    
    public static bool IsAdmin(this ClaimsPrincipal user) =>
        user.IsInRole(AppRoles.Admin);

    public static bool IsUser(this ClaimsPrincipal user) =>
        user.IsInRole(AppRoles.User);
}