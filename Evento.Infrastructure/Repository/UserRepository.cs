using Evento.Domain;
using Evento.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Evento.Infrastructure.Repository;

public class UserRepository(EventoDbContext db) : IUserRepository
{
    public Task<AppUser?> GetByIdAsync(string userId)
    {
        return db.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
}