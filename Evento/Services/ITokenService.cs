using Evento.Models;

namespace Evento.Services;

public interface ITokenService
{
    string CreateToken(AppUser user);
}