
using Evento.Infrastructure.Filters;

namespace Evento.Endpoints;

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder) where T : class
    {
        return builder.AddEndpointFilter<ValidationFilter<T>>();
    }
}