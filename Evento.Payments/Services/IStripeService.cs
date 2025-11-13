namespace Evento.Payments.Services;

public interface IStripeService
{
    Task<string> CreateUserAsync(string name, string email);
}