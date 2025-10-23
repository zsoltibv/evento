namespace Evento.Application.Common.Errors;

public static class RoleRequestErrors
{
    public static readonly Error HasActiveRequest =
        new("RoleRequestErrors.HasActiveRequest", "You already have an active request.");

    public static readonly Error NotFound =
        new("RoleRequestErrors.NotFound", "Role request not found.");

    public static readonly Error AlreadyApproved =
        new("RoleRequestErrors.AlreadyApproved", "Role request already approved.");
}