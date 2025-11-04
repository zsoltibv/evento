using Evento.Application.Common.Dto;
using Evento.Domain.Models;

namespace Evento.Application.Common.Extensions;

public static class UserExtensions
{
    public static UserDto ToDto(this AppUser user)
        => new(
            user.Id,
            user.UserName!,
            user.Email
        );
    
    public static ChatUserDto ToChatDto(this AppUser user)
        => new(
            user.Id,
            user.UserName!
        );
}