using Evento.Enums;

namespace Evento.Models;

public class Booking
{
    public int Id { get; set; }
    
    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.UtcNow; 
    
    public BookingStatus Status { get; set; }
    
    public int VenueId { get; set; }
    public Venue Venue { get; set; } = null!;
}