namespace Evento.Email.Services.Interfaces;

public interface IEmailTemplate
{
    EmailMessageDto Create(string to, object? data = null);
}