using Evento.Application.Common.Errors;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Evento.Application.Common.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(a => a is T) is not T dto)
        {
            return await next(context);
        }

        var result = await validator.ValidateAsync(dto);
        if (result.IsValid)
        {
            return await next(context);
        }
        
        var errors = result.Errors
            .Select(failure => new ErrorResponse(
                Code: failure.ErrorCode,
                Description: failure.ErrorMessage
            ))
            .ToArray();

        return Results.BadRequest(errors);
    }
}