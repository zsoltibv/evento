using Evento.Domain.Models;

namespace Evento.Application;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}