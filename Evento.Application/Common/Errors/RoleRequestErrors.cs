namespace Evento.Application.Common.Errors;

public static class RoleRequestErrors
{
    public static readonly Error HasPendingRequest =
        new("RoleRequestErrors.HasPendingRequest", "You already have a pending request.");
}