namespace Evento.Email.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessageDto message);
}