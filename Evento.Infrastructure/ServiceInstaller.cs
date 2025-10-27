using System.Security.Claims;
using Evento.Application.Services.Interfaces;
using Evento.Domain;
using Evento.Domain.Models;
using Evento.Infrastructure.Repository;
using Evento.Infrastructure.Services;
using Evento.Infrastructure.Services.EmailTemplates;
using Evento.Infrastructure.Services.Interfaces;
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
        services.AddScoped<IVenueAdminService, VenueAdminService>();
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailTemplateFactory, EmailTemplateFactory>();
        services.AddScoped<VenueAdminApprovedEmailTemplate>();

        // Register Repositories
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IRoleRequestRepository, RoleRequestRepository>();

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
                RoleClaimType = ClaimTypes.Role,
                NameClaimType = "sub"
            };
            
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/hubs/chat"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });

        // Register Authorization policies
        services.AddAuthorizationBuilder()
            .AddPolicy("User", policy => policy.RequireRole("User"))
            .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
            .AddPolicy("VenueAdmin", policy => policy.RequireRole("VenueAdmin"));

        return services;
    }
}