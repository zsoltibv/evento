namespace Evento.Application.Common.Dto;

public record BookingDetailsDto(
    int Id,
    string UserId,
    DateTime StartDate,
    DateTime EndDate,
    DateTime BookingDate,
    string Status,
    VenueDto Venue,
    UserDto User
);