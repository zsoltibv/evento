using Evento.Dto;
using Evento.Models;

namespace Evento.Extensions;

public static class VenueExtensions
{
    public static VenueDto ToDto(this Venue venue)
        => new(
            venue.Id,
            venue.Name,
            venue.Description,
            venue.Location,
            venue.Capacity,
            venue.Image 
        );

    public static IEnumerable<VenueDto> ToDto(this IEnumerable<Venue> venues)
        => venues.Select(v => v.ToDto());
}