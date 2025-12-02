using Evento.Email.EmailTemplates;
using Evento.Email.Services;
using Evento.Email.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evento.Email;

public static class ServiceInstaller
{
    public static IServiceCollection AddEmailServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Email services
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailTemplateFactory, EmailTemplateFactory>();
        
        // Email templates
        services.AddScoped<VenueAdminApprovedEmailTemplate>();
        services.AddScoped<PaymentApprovedEmailTemplate>();

        return services;
    }
}