using Evento.Application.Common.Dto;

namespace Evento.Infrastructure.Services.Interfaces;

public interface IEmailTemplate
{
    EmailMessageDto Create(string to, object? data = null);
}