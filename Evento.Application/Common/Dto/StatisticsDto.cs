namespace Evento.Application.Common.Dto;

public record StatisticsDto(
    int BookingsCount,
    int VenuesCount,
    decimal TotalRevenue,
    decimal[] WeeklyRevenue,
    IEnumerable<VenueStatisticDto>? VenueStatistics // null for non-admins
);

public record VenueStatisticDto(
    int VenueId,
    string VenueName,
    int BookingsCount,
    decimal TotalRevenue
);