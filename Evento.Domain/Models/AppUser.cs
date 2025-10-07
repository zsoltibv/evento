using Microsoft.AspNetCore.Identity;

namespace Evento.Domain.Models;

public class AppUser : IdentityUser
{
    public ICollection<VenueAdmin> VenueAdmins { get; set; } = new List<VenueAdmin>();
}