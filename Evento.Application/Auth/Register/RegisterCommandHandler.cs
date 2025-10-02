using Evento.Application.Common;
using Evento.Application.Common.Dto;
using Evento.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Evento.Application.Auth.Register;

public class RegisterCommandHandler(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    IValidator<RegisterDto> validator)
    : ICommandHandler<RegisterCommand>
{
    public async Task<IResult> Handle(RegisterCommand command)
    {
        var validation = await validator.ValidateAsync(command.Dto);
        if (!validation.IsValid)
        {
            return Results.BadRequest(validation.Errors);
        }

        var appUser = new AppUser
        {
            UserName = command.Dto.Username,
            Email = command.Dto.Email
        };

        var createResult = await userManager.CreateAsync(appUser, command.Dto.Password);
        if (!createResult.Succeeded)
        {
            return Results.BadRequest(createResult.Errors);
        }

        var roleResult = await userManager.AddToRoleAsync(appUser, "User");
        if (!roleResult.Succeeded)
        {
            return Results.BadRequest(roleResult.Errors);
        }

        var token = await tokenService.CreateToken(appUser);
        return Results.Ok(new NewUserDto(appUser.UserName, appUser.Email, token));
    }
}