namespace Evento.Application.Common.Dto;

public record EmailMessageDto(string To, string Subject, string Body, bool IsHtml = true);