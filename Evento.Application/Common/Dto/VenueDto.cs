namespace Evento.Application.Common.Dto;

public record VenueDto(
    int Id,
    string Name,
    string? Description,
    string Location,
    int Capacity,
    string? ImageUrl
);

public record VenueWithBookingsDto(
    int Id,
    string Name,
    string? Description,
    string Location,
    int Capacity,
    string? ImageUrl,
    IEnumerable<BookingWithVenueNameDto> Bookings
);