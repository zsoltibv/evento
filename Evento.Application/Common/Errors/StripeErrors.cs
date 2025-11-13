namespace Evento.Application.Common.Errors;

public static class StripeErrors
{
    public static readonly Error FailedCreatingCustomer =
        new("RoleRequestErrors.FailedCreatingCustomer", "Customer creation failed.");
}