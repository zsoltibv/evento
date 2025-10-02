using System.Security.Claims;
using Evento.Application;
using Evento.Application.Bookings;
using Evento.Application.Venues;
using Evento.Domain;
using Evento.Domain.Models;
using Evento.Infrastructure.Repository;
using Evento.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Evento.Infrastructure;

public static class ServiceInstaller
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<EventoDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Register Services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IVenueService, VenueService>();
        services.AddScoped<IBookingService, BookingService>();

        // Register Repositories
        services.AddScoped<IBookingRepository, BookingRepository>();

        // Register Identity
        services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 12;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<EventoDbContext>();

        // Register Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]!)),
                RoleClaimType = ClaimTypes.Role
            };
        });

        // Register Authorization policies
        services.AddAuthorizationBuilder()
            .AddPolicy("User", policy => policy.RequireRole("User"))
            .AddPolicy("Admin", policy => policy.RequireRole("Admin"));

        return services;
    }
}