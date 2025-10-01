using Evento.Dto;
using Evento.Errors;
using Evento.Extensions;
using FluentValidation;

namespace Evento.Validators;

internal sealed class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithError(AuthErrors.EmailIsEmpty);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithError(AuthErrors.PasswordIsEmpty);
    }
}