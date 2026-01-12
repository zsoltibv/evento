namespace Evento.Application.Common.Dto;

public record StatisticsDto(
    int BookingsCount,
    int VenuesCount,
    decimal TotalRevenue,
    decimal[] WeeklyRevenue
);