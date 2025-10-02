using Evento.Application.Common;
using Evento.Application.Common.Dto;
using Evento.Application.Common.Errors;
using Evento.Application.Venues;
using FluentValidation;

namespace Evento.Application.Bookings.UpdateBooking;

public sealed class UpdateBookingDtoValidator : AbstractValidator<UpdateBookingDto>
{
    public UpdateBookingDtoValidator(IVenueService venueService)
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

        RuleFor(x => x.Status)
            .NotNull().WithError(BookingErrors.StatusRequired)
            .IsInEnum().WithError(BookingErrors.InvalidStatus);
    }
}