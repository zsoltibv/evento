using Evento.Models;

namespace Evento.Services;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}