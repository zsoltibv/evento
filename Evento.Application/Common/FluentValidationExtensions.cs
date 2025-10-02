using Evento.Application.Common.Errors;
using FluentValidation;

namespace Evento.Application.Common;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule,
        ErrorResponse error)
        => rule.WithErrorCode(error.Code).WithMessage(error.Description).WithState(_ => error);
}