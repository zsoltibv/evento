namespace Evento.Application.Services.Interfaces;

public interface IUserService
{
    Task<string?> GetCustomerId(string userId);
}