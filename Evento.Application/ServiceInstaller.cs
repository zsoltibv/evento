using System.Reflection;
using Evento.Application.Auth.Login;
using Evento.Application.Common;
using Evento.Application.Services;
using Evento.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Evento.Application;

public static class ServiceInstaller
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<LoginQueryHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(LoginDtoValidator))!, includeInternalTypes: true);

        // Register Service 
        services.AddScoped<IRoleRequestService, RoleRequestService>();
        services.AddScoped<IChatService, ChatService>();
            
        return services;
    }
}