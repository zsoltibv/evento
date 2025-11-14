namespace Evento.Application.Common.Dto;

public record CreateIntentRequest(string CustomerId, decimal PricePerHour, int Hours);