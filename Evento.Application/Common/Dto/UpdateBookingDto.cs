namespace Evento.Application.Common.Dto;

public record UpdateBookingDto(
    DateTime? StartDate,
    DateTime? EndDate,
    int? VenueId,
    string? Status
);