using Bogus;
using Evento.Payments.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stripe;
using ProductService = Stripe.Climate.ProductService;

namespace Evento.Payments;

public static class ServiceInstaller
{
    public static IServiceCollection AddPaymentServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // My Services
        services.AddScoped<IStripeService, StripeService>();
        services.AddSingleton<Faker>();
        
        // Settings 
        services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));
        
        // Stripe Services
        services.AddScoped<IStripeClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<StripeSettings>>().Value;
            return new StripeClient(settings.SecretKey);
        });
        services.AddScoped<CustomerService>();
        services.AddScoped<ProductService>();
        
        return services;
    }
} 