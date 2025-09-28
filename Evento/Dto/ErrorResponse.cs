namespace Evento.Dto;

public record ErrorResponse(string? Message = null, object? Errors = null);