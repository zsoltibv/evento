using Evento.Application.Common.Dto;
using Evento.Domain.Models;

namespace Evento.Application.Common.Extensions;

public static class VenueExtensions
{
    public static VenueDto ToDto(this Venue venue)
        => new(
            venue.Id,
            venue.Name,
            venue.Description,
            venue.Location,
            venue.Capacity,
            venue.ImageUrl
        );

    public static VenueWithBookingsDto ToDtoWithBookings(this Venue venue)
        => new(
            venue.Id,
            venue.Name,
            venue.Description,
            venue.Location,
            venue.Capacity,
            venue.ImageUrl,
            venue.Bookings.ToDtoWithVenueName()
        );

    public static IEnumerable<VenueDto> ToDto(this IEnumerable<Venue> venues)
        => venues.Select(v => v.ToDto());
}