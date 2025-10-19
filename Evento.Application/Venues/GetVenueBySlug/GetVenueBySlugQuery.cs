using Evento.Application.Common;

namespace Evento.Application.Venues.GetVenueBySlug;

public record GetVenueBySlugQuery(string Slug) : IQuery;