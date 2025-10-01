using Evento.Errors;
using FluentValidation;

namespace Evento.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule,
        ErrorResponse error)
        => rule.WithErrorCode(error.Code).WithMessage(error.Description).WithState(_ => error);
}