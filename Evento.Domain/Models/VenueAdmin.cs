namespace Evento.Domain.Models;

public class VenueAdmin
{
    public int VenueId { get; set; }
    public Venue Venue { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;
}