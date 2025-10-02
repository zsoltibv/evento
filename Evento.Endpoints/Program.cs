using System.Reflection;
using System.Security.Claims;
using Evento.Application;
using Evento.Application.Auth.Login;
using Evento.Application.Bookings;
using Evento.Application.Venues;
using Evento.Domain;
using Evento.Domain.Common;
using Evento.Domain.Models;
using Evento.Endpoints;
using Evento.Endpoints.Endpoints;
using Evento.Infrastructure;
using Evento.Infrastructure.Repository;
using Evento.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(LoginDtoValidator))!, includeInternalTypes: true);

builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

// Register DB
var connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddDbContext<EventoDbContext>(options =>
    options.UseNpgsql(connectionString));

//Register Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<IBookingService, BookingService>();

//Register Repositories
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

//Register Handlers and Queries
builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(Program), typeof(LoginQueryHandler))
    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);

// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:4200") // Angular dev server URL
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//Register Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 12;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<EventoDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!)),
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("User", policy =>
        policy.RequireRole("User"))
    .AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

// Map Endpoints
app.MapAuthEndpoints();
app.MapVenueEndpoints();
app.MapBookingEndpoints();

app.Run();