using Evento.Application.Services.Interfaces;
using Evento.Domain;

namespace Evento.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<string?> GetCustomerId(string userId) => await userRepository.GetCustomerId(userId);
}