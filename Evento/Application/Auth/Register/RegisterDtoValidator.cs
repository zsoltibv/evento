using Evento.Dto;
using Evento.Errors;
using Evento.Extensions;
using FluentValidation;

namespace Evento.Application.Auth.Register;

internal sealed class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithError(AuthErrors.UsernameIsEmpty);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithError(AuthErrors.EmailIsEmpty)
            .EmailAddress()
            .WithError(AuthErrors.EmailInvalid);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithError(AuthErrors.PasswordIsEmpty);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithError(AuthErrors.ConfirmPasswordIsEmpty)
            .Equal(x => x.Password)
            .WithError(AuthErrors.PasswordsMismatch);
    }
}