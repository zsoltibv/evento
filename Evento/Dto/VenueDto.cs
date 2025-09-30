namespace Evento.Dto;

public record VenueDto(
    int Id,
    string Name,
    string? Description,
    string Location,
    int Capacity,
    byte[]? Image
);