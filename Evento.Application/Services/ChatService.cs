using Evento.Application.Common.Dto;
using Evento.Application.Services.Interfaces;
using Evento.Domain;
using Evento.Domain.Models;

namespace Evento.Application.Services;

public sealed class ChatService(IChatRepository chatRepository, IChatClaimRepository chatClaimRepository, IUserRepository userRepository) : IChatService
{
    public async Task<ChatMessage> SendMessageAsync(string senderId, string receiverId, string messageText)
    {
        if (string.IsNullOrWhiteSpace(senderId) ||
            string.IsNullOrWhiteSpace(receiverId) ||
            string.IsNullOrWhiteSpace(messageText))
        {
            throw new ArgumentException("Sender, receiver, and message text must be provided.");
        }

        var message = new ChatMessage
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            MessageText = messageText,
            SentAt = DateTime.UtcNow
        };

        await chatRepository.SaveMessageAsync(message);
        return message;
    }

    public async Task<List<ChatMessage>> GetChatHistoryAsync(string userId1, string userId2)
    {
        return await chatRepository.GetChatHistoryAsync(userId1, userId2);
    }

    public async Task<bool> TryClaimChatAsync(string userId, string agentId)
    {
        var existingClaim = await chatClaimRepository.GetByUserIdAsync(userId);
        if (existingClaim != null)
            return false;

        await chatClaimRepository.AddAsync(new ChatClaim
        {
            UserId = userId,
            AgentId = agentId
        });
        
        return true;
    }

    public async Task<ChatClaimOwnerDto?> GetChatClaimOwnerAsync(string userId)
    {
        var claim = await chatClaimRepository.GetByAgentIdAsync(userId);
        if (claim == null) return null;

        var agent = await userRepository.GetByIdAsync(claim.AgentId); 
        return new ChatClaimOwnerDto
        {
            AgentId = claim.AgentId,
            AgentName = agent?.UserName ?? "Unknown"
        };
    }
}