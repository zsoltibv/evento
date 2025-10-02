using Evento.Domain.Enums;

namespace Evento.Application.Common.Dto;

public record UpdateBookingDto(
    DateTime? StartDate,
    DateTime? EndDate,
    int? VenueId,
    BookingStatus? Status
);