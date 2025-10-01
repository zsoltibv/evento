using Evento.Dto;
using Evento.Errors;
using Evento.Extensions;
using Evento.Services;
using FluentValidation;

namespace Evento.Validators;

internal sealed class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
{
    public CreateBookingDtoValidator(IVenueService venueService)
    {
        RuleFor(x => x.StartDate)
            .NotNull().WithError(BookingErrors.StartDateRequired)
            .LessThan(x => x.EndDate).WithError(BookingErrors.StartDateBeforeEndDate)
            .GreaterThan(DateTime.Now).WithError(BookingErrors.StartDateInFuture);

        RuleFor(x => x.EndDate)
            .NotNull().WithError(BookingErrors.EndDateRequired)
            .GreaterThan(x => x.StartDate).WithError(BookingErrors.EndDateAfterStartDate)
            .GreaterThan(DateTime.Now).WithError(BookingErrors.EndDateInFuture);

        RuleFor(x => x.VenueId)
            .NotNull().WithError(BookingErrors.VenueRequired)
            .MustAsync(async (venueId, ct) => await venueService.ExistsAsync(venueId!.Value))
            .WithError(BookingErrors.VenueNotFound);
    }
}