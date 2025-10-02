using Evento.Application.Common;

namespace Evento.Application.Venues.GetVenueById;

public record GetVenueByIdQuery(int VenueId) : IQuery;