namespace Evento.Email;

public record EmailMessageDto(string To, string Subject, string Body, bool IsHtml = true);