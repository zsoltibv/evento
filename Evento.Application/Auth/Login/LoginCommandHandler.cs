using Evento.Application.Common.Dto;
using Evento.Application.Common.Errors;
using Evento.Domain.Common;
using Evento.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Evento.Application.Auth.Login;

public class LoginQueryHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ITokenService tokenService)
    : IQueryHandler<LoginQuery>
{
    public async Task<IResult> Handle(LoginQuery query)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == query.Dto.Email);
        if (user == null)
        {
            return Results.Json(AuthErrors.EmailInvalid, statusCode: StatusCodes.Status401Unauthorized);
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, query.Dto.Password, false);
        if (!result.Succeeded)
        {
            return Results.Json(AuthErrors.EmailOrPasswordIncorrect, statusCode: StatusCodes.Status401Unauthorized);
        }

        var token = await tokenService.CreateToken(user);
        return Results.Ok(new NewUserDto(user.UserName!, user.Email!, token));
    }
}