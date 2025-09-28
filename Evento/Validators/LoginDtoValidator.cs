using Evento.Dto;
using FluentValidation;

namespace Evento.Validators;

internal sealed class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}