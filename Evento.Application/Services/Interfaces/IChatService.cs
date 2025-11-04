using Evento.Application.Common.Dto;
using Evento.Domain.Models;

namespace Evento.Application.Services.Interfaces;

public interface IChatService
{
    Task<ChatMessage> SendMessageAsync(string senderId, string receiverId, string messageText);
    Task<List<ChatMessage>> GetChatHistoryAsync(string userId1, string userId2);
    Task<ChatClaimOwnerDto?> TryClaimChatAsync(string userId, string agentId);
    Task<ChatClaimOwnerDto?> GetChatClaimOwnerAsync(string userId);
    Task<IEnumerable<ChatUserDto>> GetUserChatsAsync(string userId);
}