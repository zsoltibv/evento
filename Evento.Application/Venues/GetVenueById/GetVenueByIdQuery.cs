using Evento.Domain.Common;

namespace Evento.Application.Venues.GetVenueById;

public record GetVenueByIdQuery(int VenueId) : IQuery;