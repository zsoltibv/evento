using Bogus;
using Microsoft.Extensions.Options;
using Stripe;

namespace Evento.Payments.Services;

public class StripeService(
    CustomerService customerService,
    PaymentIntentService paymentIntentService,
    Faker faker,
    IOptions<StripeSettings> options)
    : IPaymentService
{
    private readonly StripeSettings _options = options.Value;

    public async Task<string> CreateUserAsync(string name, string email)
    {
        try
        {
            StripeConfiguration.ApiKey = _options.SecretKey;

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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return string.Empty;
        }
    }
    
    public async Task<string> CreateVenuePaymentIntentAsync(
        string customerId,
        decimal pricePerHour,
        int hours)
    {
        StripeConfiguration.ApiKey = _options.SecretKey;

        var amountRon = pricePerHour * hours;
        var amountBani = (long)(amountRon * 100);

        var ccOptions = new PaymentIntentCreateOptions
        {
            Amount = amountBani,
            Currency = "ron",
            Customer = customerId,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            },
            Description = $"Venue booking: {hours}h × {pricePerHour} RON"
        };
        
        var intent = await paymentIntentService.CreateAsync(ccOptions);
        return intent.ClientSecret;
    }
}