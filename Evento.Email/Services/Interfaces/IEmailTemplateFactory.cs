namespace Evento.Email.Services.Interfaces;

public interface IEmailTemplateFactory
{
    EmailMessageDto CreateEmail<TTemplate>(string to, object? data = null) where TTemplate : IEmailTemplate;
}