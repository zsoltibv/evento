using Bogus;
using Stripe;

namespace Evento.Payments.Services;

public class StripeService(IStripeClient stripeClient, CustomerService customerService, Faker faker) : IStripeService
{
    public async Task<string> CreateUserAsync(string name, string email)
    {
        var ccOptions = new CustomerCreateOptions
        {
            Name = name,
            Email = email,
            Description = "Fake User Account",
            PaymentMethod = "pm_card_visa",
            Address = new AddressOptions
            {
                Line1 = faker.Address.StreetAddress(),
                City = faker.Address.City(),
                State = faker.Address.State(),
                Country = "RO"
            },
            InvoiceSettings = new CustomerInvoiceSettingsOptions { DefaultPaymentMethod = "pm_card_visa" },
        };

        var customer = await customerService.CreateAsync(ccOptions);
        return customer.Id;
    }
}