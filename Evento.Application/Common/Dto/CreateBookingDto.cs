namespace Evento.Application.Common.Dto;

public record CreateBookingDto(
    DateTime? StartDate,
    DateTime? EndDate,
    int? VenueId
);