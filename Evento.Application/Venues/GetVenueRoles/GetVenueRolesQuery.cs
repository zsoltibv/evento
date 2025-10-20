using Evento.Application.Common;

namespace Evento.Application.Venues.GetVenueRoles;

public record GetVenueRolesQuery(string UserId, bool IsAdmin) : IQuery;