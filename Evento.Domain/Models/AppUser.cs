using Microsoft.AspNetCore.Identity;

namespace Evento.Domain.Models;

public class AppUser : IdentityUser
{
    public string StripeCustomerId { get; set; } = string.Empty;
    public ICollection<VenueAdmin> VenueAdmins { get; set; } = new List<VenueAdmin>();
}