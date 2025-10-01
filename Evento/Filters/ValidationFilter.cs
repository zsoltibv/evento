using FluentValidation;

namespace Evento.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(a => a is T) is not T dto)
        {
            return await next(context);
        }
        
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            return Results.BadRequest(new { Errors = result.ToDictionary() });
        }

        return await next(context);
    }
}