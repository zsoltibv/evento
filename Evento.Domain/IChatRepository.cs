using Evento.Domain.Models;

namespace Evento.Domain;

public interface IChatRepository
{
    Task SaveMessageAsync(ChatMessage message);
    Task<List<ChatMessage>> GetChatHistoryAsync(string userId1, string userId2);
    Task<IEnumerable<AppUser>> GetUserChatsAsync(string userId);
}