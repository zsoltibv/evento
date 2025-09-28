namespace Evento.Models;

public class Booking
{
    public int Id { get; set; }
    
    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime BookingDate { get; set; }
    
    public BookingStatus Status { get; set; }
}