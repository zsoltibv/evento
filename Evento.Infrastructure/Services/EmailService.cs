using System.Net;
using System.Net.Mail;
using Evento.Application.Common.Dto;
using Evento.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Evento.Infrastructure.Services;

public class EmailService(IOptions<EmailSettings> options) : IEmailService
{
    private readonly EmailSettings _settings = options.Value;

    public async Task SendEmailAsync(EmailMessageDto message)
    {
        using var client = new SmtpClient(_settings.SmtpServer, _settings.Port);
        client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
        client.EnableSsl = _settings.EnableSsl;

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = message.Subject,
            Body = message.Body,
            IsBodyHtml = message.IsHtml
        };
        mailMessage.To.Add(message.To);

        await client.SendMailAsync(mailMessage);
    }
}