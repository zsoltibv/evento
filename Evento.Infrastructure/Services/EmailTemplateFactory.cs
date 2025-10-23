using Evento.Application.Common.Dto;
using Evento.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Evento.Infrastructure.Services;

public class EmailTemplateFactory(IServiceProvider serviceProvider) : IEmailTemplateFactory
{
    public EmailMessageDto CreateEmail<TTemplate>(string to, object? data = null)
        where TTemplate : IEmailTemplate
    {
        var template = serviceProvider.GetRequiredService<TTemplate>();
        return template.Create(to, data);
    }
}