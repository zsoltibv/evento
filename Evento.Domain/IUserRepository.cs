using Evento.Domain.Models;

namespace Evento.Domain;

public interface IUserRepository
{
    Task<AppUser?> GetByIdAsync(string userId);
    Task<string?> GetCustomerId(string userId);
}