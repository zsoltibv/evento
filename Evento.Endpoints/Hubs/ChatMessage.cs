namespace Evento.Endpoints.Hubs;

public record ChatMessage(ChatUser Sender, ChatUser Receiver, string Message);