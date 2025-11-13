namespace Evento.Payments.Services;

public interface IPaymentService
{
    Task<string> CreateUserAsync(string name, string email);
}