using Evento.Application.Common.Dto;

namespace Evento.Infrastructure.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessageDto message);
}