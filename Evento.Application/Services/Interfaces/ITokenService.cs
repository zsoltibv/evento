using Evento.Domain.Models;

namespace Evento.Application.Services.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}