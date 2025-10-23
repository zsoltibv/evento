using Evento.Application.Common.Dto;

namespace Evento.Infrastructure.Services.Interfaces;

public interface IEmailTemplateFactory
{
    EmailMessageDto CreateEmail<TTemplate>(string to, object? data = null) where TTemplate : IEmailTemplate;
}