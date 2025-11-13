using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using ProductService = Stripe.Climate.ProductService;

namespace Evento.Payments;

public static class ServiceInstaller
{
    public static IServiceCollection AddPaymentServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Settings 
        services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));
        
        // Services
        services.AddScoped<CustomerService>();
        services.AddScoped<ProductService>();
        
        return services;
    }
}