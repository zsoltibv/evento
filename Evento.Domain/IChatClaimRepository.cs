using Evento.Domain.Models;

namespace Evento.Domain;

public interface IChatClaimRepository
{
    Task<ChatClaim?> GetByUserIdAsync(string userId);
    Task AddAsync(ChatClaim claim);
}