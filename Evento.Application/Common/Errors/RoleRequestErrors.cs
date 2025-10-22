namespace Evento.Application.Common.Errors;

public static class RoleRequestErrors
{
    public static readonly Error HasPendingRequest =
        new("RoleRequestErrors.HasPendingRequest", "You already have a pending request.");
    
    public static readonly Error NotFound =
        new("RoleRequestErrors.NotFound", "Role request not found.");
    
    public static readonly Error AlreadyApproved =
            new("RoleRequestErrors.AlreadyApproved", "Role request already approved.");
}