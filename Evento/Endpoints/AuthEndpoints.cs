using Evento.Dto;
using Evento.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Evento.Endpoints;

public static class AuthEndpoints
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/api/auth");

        authGroup.MapPost("/register",
            async (RegisterDto registerDto, UserManager<AppUser> userManager, IValidator<RegisterDto> validator) =>
            {
                try
                {
                    var validationResult = await validator.ValidateAsync(registerDto);

                    if (!validationResult.IsValid)
                    {
                        return Results.BadRequest(new ErrorResponse("Validation failed",
                            validationResult.ToDictionary()));
                    }

                    var appUser = new AppUser
                    {
                        UserName = registerDto.Username,
                        Email = registerDto.Email,
                    };

                    var createUser = await userManager.CreateAsync(appUser, registerDto.Password);

                    if (!createUser.Succeeded)
                    {
                        return Results.BadRequest(createUser.Errors);
                    }

                    var roleResult = await userManager.AddToRoleAsync(appUser, "User");
                    return roleResult.Succeeded ? Results.Ok("User created") : Results.BadRequest(roleResult.Errors);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

        return app;
    }
}