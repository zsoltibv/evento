using Evento.Enums;

namespace Evento.Dto;

public record UpdateBookingDto(
    DateTime? StartDate,
    DateTime? EndDate,
    int? VenueId,
    BookingStatus? Status
);