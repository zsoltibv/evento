using Evento.Application.Common;
using Evento.Application.Common.Dto;
using Evento.Application.Common.Errors;
using FluentValidation;

namespace Evento.Application.Auth.Login;

public sealed class LoginDtoValidator : AbstractValidator<LoginDto>
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