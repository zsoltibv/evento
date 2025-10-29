using Evento.Domain;
using Evento.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Evento.Infrastructure.Repository;

public sealed class ChatRepository(EventoDbContext db) : IChatRepository
{
    public async Task SaveMessageAsync(ChatMessage message)
    {
        db.ChatMessages.Add(message);
        await db.SaveChangesAsync();
    }
    
    public async Task<List<ChatMessage>> GetChatHistoryAsync(string userId1, string userId2)
    {
        return await db.ChatMessages
            .Where(m =>
                (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                (m.SenderId == userId2 && m.ReceiverId == userId1))
            .OrderBy(m => m.SentAt)
            .AsNoTracking()
            .ToListAsync();
    }
}