using Bogus;
using Evento.Payments.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Stripe.Checkout;
using ProductService = Stripe.Climate.ProductService;

namespace Evento.Payments;

public static class ServiceInstaller
{
    public static IServiceCollection AddPaymentServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // My Services
        services.AddScoped<IPaymentService, StripeService>();
        services.AddSingleton<Faker>();
        
        // Settings 
        services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));
        
        // Stripe Services
        services.AddScoped<CustomerService>();
        services.AddScoped<ProductService>();
        services.AddScoped<PaymentIntentService>();
        services.AddScoped<SessionService>();
        
        return services;
    }
} 