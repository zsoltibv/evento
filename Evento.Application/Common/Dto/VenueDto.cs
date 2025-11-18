namespace Evento.Application.Common.Dto;

public record VenueDto(
    int Id,
    string Name,
    string? Description,
    string Location,
    int Capacity,
    string? ImageUrl,
    string Slug,
    decimal PricePerHour
);

public record VenueWithBookingsDto(
    int Id,
    string Name,
    string? Description,
    string Location,
    int Capacity,
    string? ImageUrl,
    string Slug,
    decimal PricePerHour,
    IEnumerable<BookingWithInfo> Bookings
);