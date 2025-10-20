using Evento.Domain.Enums;

namespace Evento.Domain.Models;

public class RoleRequest
{
    public int Id { get; set; } 

    public string UserId { get; set; } = null!; 
    public AppUser User { get; set; } = null!;

    public string RoleName { get; set; } = null!;
    public RequestStatus Status { get; set; } = RequestStatus.Pending; 
    public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    
    public int? VenueId { get; set; } 
    public Venue? Venue { get; set; }
}