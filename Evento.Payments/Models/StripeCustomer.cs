namespace Evento.Payments.Models;

public class StripeCustomer
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}