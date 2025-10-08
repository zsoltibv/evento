using Evento.Application.Common;
using Evento.Application.Common.Dto;
using Evento.Application.Common.Errors;
using Evento.Application.Venues;
using Evento.Domain.Enums;
using FluentValidation;

namespace Evento.Application.Bookings.UpdateBooking;

public sealed class UpdateBookingDtoValidator : AbstractValidator<UpdateBookingDto>
{
    public UpdateBookingDtoValidator(IVenueService venueService)
    {
        When(x => x.StartDate.HasValue && x.EndDate.HasValue, () =>
        {
            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate!.Value)
                .WithError(BookingErrors.StartDateBeforeEndDate);
        });

        When(x => x.StartDate.HasValue, () =>
        {
            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.UtcNow.AddMinutes(-1))
                .WithError(BookingErrors.StartDateInFuture);
        });
        
        When(x => x.EndDate.HasValue && x.StartDate.HasValue, () =>
        {
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate!.Value)
                .WithError(BookingErrors.EndDateAfterStartDate);
        });

        When(x => x.EndDate.HasValue, () =>
        {
            RuleFor(x => x.EndDate)
                .GreaterThan(DateTime.UtcNow.AddMinutes(-1))
                .WithError(BookingErrors.EndDateInFuture);
        });
        
        When(x => x.VenueId.HasValue, () =>
        {
            RuleFor(x => x.VenueId)
                .MustAsync(async (venueId, ct) =>
                    await venueService.ExistsAsync(venueId!.Value))
                .WithError(BookingErrors.VenueNotFound);
        });

        When(x => !string.IsNullOrWhiteSpace(x.Status), () =>
        {
            RuleFor(x => x.Status!)
                .Must(status => status.TryToBookingStatus(out _))
                .WithError(BookingErrors.InvalidStatus);
        });
    }
}