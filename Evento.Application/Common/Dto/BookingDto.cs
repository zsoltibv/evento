namespace Evento.Application.Common.Dto;

public record BookingDto(
    int Id,
    string UserId,
    DateTime StartDate,
    DateTime EndDate,
    DateTime BookingDate,
    string Status,
    int VenueId
);