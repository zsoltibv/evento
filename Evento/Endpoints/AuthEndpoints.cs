using Evento.Dto;
using Evento.Extensions;
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
                            new NewUserDto(appUser.UserName, appUser.Email, await tokenService.CreateToken(appUser))
                        )
                        : Results.BadRequest(roleResult.Errors);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithValidation<RegisterDto>()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        authGroup.MapPost("/login", async (LoginDto loginDto, IValidator<LoginDto> validator,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService) =>
        {
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

            return Results.Ok(new NewUserDto(user.UserName!, user.Email!, await tokenService.CreateToken(user)));
        })
        .WithValidation<LoginDto>()
        .Produces(StatusCodes.Status200OK)       
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);
        
        return app;
    }
}