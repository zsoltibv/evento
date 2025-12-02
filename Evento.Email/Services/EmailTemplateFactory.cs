using Evento.Email.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Evento.Email.Services;

public class EmailTemplateFactory(IServiceProvider serviceProvider) : IEmailTemplateFactory
{
    public EmailMessageDto CreateEmail<TTemplate>(string to, object? data = null)
        where TTemplate : IEmailTemplate
    {
        var template = serviceProvider.GetRequiredService<TTemplate>();
        return template.Create(to, data);
    }
}