using Evento.Application.Common;
using Evento.Application.Common.Dto;
using Evento.Application.Common.Errors;
using Evento.Application.Services.Interfaces;
using Evento.Domain.Models;
using Evento.Payments.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Evento.Application.Auth.Register;

public class RegisterCommandHandler(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    IPaymentService paymentService)
    : ICommandHandler<RegisterCommand>
{
    public async Task<IResult> Handle(RegisterCommand command)
    {
        var stripeCustomerId = await paymentService.CreateUserAsync(command.Dto.Username, command.Dto.Email);
        if (string.IsNullOrEmpty(stripeCustomerId))
        {
            return Results.BadRequest(StripeErrors.FailedCreatingCustomer);
        }
        
        var appUser = new AppUser
        {
            UserName = command.Dto.Username,
            Email = command.Dto.Email,
            StripeCustomerId = stripeCustomerId
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