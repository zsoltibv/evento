using Evento.Domain.Models;

namespace Evento.Domain;

public interface IChatClaimRepository
{
    Task<ChatClaim?> GetByUserIdAsync(string userId);
    Task<ChatClaim?> GetByAgentIdAsync(string agentId);
    Task AddAsync(ChatClaim claim);
}