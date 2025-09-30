namespace Evento.Dto;

public record CreateBookingDto(
    DateTime StartDate,
    DateTime EndDate,
    int VenueId
);