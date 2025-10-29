namespace Evento.Application.Common.Dto;

public record ChatMessageDto(
    ChatUserDto Sender,
    ChatUserDto Receiver,
    string MessageText,
    DateTime SentAt
);