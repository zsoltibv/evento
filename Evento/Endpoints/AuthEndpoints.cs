using Evento.Dto;
using Evento.Models;
using Evento.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Evento.Endpoints;

public static class AuthEndpoints
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/api/auth");

        authGroup.MapPost("/register",
            async (RegisterDto registerDto, UserManager<AppUser> userManager, IValidator<RegisterDto> validator,
                ITokenService tokenService) =>
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
                    return roleResult.Succeeded
                        ? Results.Ok(
                            new NewUserDto(appUser.UserName, appUser.Email, tokenService.CreateToken(appUser))
                        )
                        : Results.BadRequest(roleResult.Errors);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

        authGroup.MapPost("/login", async (LoginDto loginDto, IValidator<LoginDto> validator,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService) =>
        {
            var validationResult = await validator.ValidateAsync(loginDto);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(new ErrorResponse("Validation failed",
                    validationResult.ToDictionary()));
            }

            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null)
            {
                return Results.Json(new ErrorResponse("Invalid email"),
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Results.Json(new ErrorResponse("Email not found and/or password is incorrect"),
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            return Results.Ok(new NewUserDto(user.UserName!, user.Email!, tokenService.CreateToken(user)));
        });

        return app;
    }
}