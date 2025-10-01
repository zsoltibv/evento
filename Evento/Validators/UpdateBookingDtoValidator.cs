using Evento.Dto;
using FluentValidation;

namespace Evento.Validators;

internal sealed class UpdateBookingDtoValidator : AbstractValidator<UpdateBookingDto>
{
    public UpdateBookingDtoValidator()
    {
        RuleFor(x => x.StartDate)
            .NotNull().WithMessage("Start date is required")
            .LessThan(x => x.EndDate).WithMessage("Start date must be before end date")
            .GreaterThan(DateTime.Now).WithMessage("Start date must be in the future");

        RuleFor(x => x.EndDate)
            .NotNull().WithMessage("End date is required")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date")
            .GreaterThan(DateTime.Now).WithMessage("End date must be in the future");

        RuleFor(x => x.VenueId)
            .NotNull().WithMessage("Venue id is required");

        RuleFor(x => x.Status)
            .NotNull().WithMessage("Status is required")
            .IsInEnum().WithMessage("Invalid booking status");
    }
}