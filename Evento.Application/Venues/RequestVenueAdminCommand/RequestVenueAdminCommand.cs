using Evento.Application.Common;

namespace Evento.Application.Venues.RequestVenueAdminCommand;

public record RequestVenueAdminCommand(string UserId, int VenueId) : ICommand;