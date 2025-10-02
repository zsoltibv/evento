using Evento.Application.Common.Filters;

namespace Evento.Endpoints.Helpers;

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder) where T : class
    {
        return builder.AddEndpointFilter<ValidationFilter<T>>();
    }
}