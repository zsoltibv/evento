namespace Evento.Application.Common.Dto;

public record VenueDto(
    int Id,
    string Name,
    string? Description,
    string Location,
    int Capacity,
    string? ImageUrl,
    string Slug
);

public record VenueWithBookingsDto(
    int Id,
    string Name,
    string? Description,
    string Location,
    int Capacity,
    string? ImageUrl,
    string Slug,
    IEnumerable<BookingWithVenueNameDto> Bookings
);