namespace Evento.Payments.Services;

public interface IPaymentService
{
    Task<string> CreateUserAsync(string name, string email);

    Task<string> CreateVenuePaymentIntentAsync(
        string customerId,
        decimal pricePerHour,
        int hours);
}