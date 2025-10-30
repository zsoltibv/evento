using Evento.Domain;
using Evento.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Evento.Infrastructure.Repository;

public class ChatClaimRepository(EventoDbContext db) : IChatClaimRepository
{
    public async Task<ChatClaim?> GetByUserIdAsync(string userId)
    {
        return await db.ChatClaims.FirstOrDefaultAsync(c => c.UserId == userId);     
    }

    public async Task AddAsync(ChatClaim claim)
    {
        await db.ChatClaims.AddAsync(claim);    
        await db.SaveChangesAsync();
    }
}