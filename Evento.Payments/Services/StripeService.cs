using Bogus;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Evento.Payments.Services;

public class StripeService(
    CustomerService customerService,
    SessionService sessionService,
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

    public async Task<string> CreateCheckoutSessionAsync(
        string customerId,
        decimal pricePerHour,
        int minutes,
        int bookingId)
    {
        StripeConfiguration.ApiKey = _options.SecretKey;

        var hours = minutes / 60m;
        var amountRon = pricePerHour * hours;
        var amountBani = (long)(amountRon * 100);

        var ccOptions = new SessionCreateOptions
        {
            Customer = customerId,
            Mode = "payment",
            UiMode = "embedded",
            ClientReferenceId = bookingId.ToString(),
            ReturnUrl = "http://localhost:4200/payment-result?sessionId={CHECKOUT_SESSION_ID}",

            LineItems =
            [
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "ron",
                        UnitAmount = amountBani,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Venue booking"
                        }
                    }
                }
            ]
        };

        var session = await sessionService.CreateAsync(ccOptions);
        return session.ClientSecret;
    }

    public async Task<StripeSessionStatus> GetStripeSessionStatusAsync(string sessionId)
    {
        var session = await sessionService.GetAsync(sessionId);
        var amountPaid = session.AmountTotal.GetValueOrDefault() / 100m;

        return new StripeSessionStatus(
            session.Status,
            session.CustomerDetails!.Email,
            amountPaid,
            int.Parse(session.ClientReferenceId!)
        );
    }
}