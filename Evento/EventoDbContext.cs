using Evento.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Evento;

public class EventoDbContext(DbContextOptions<EventoDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        List<IdentityRole> roles =
        [
            new()
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new()
            {
                Name = "User",
                NormalizedName = "USER"
            }
        ];
        modelBuilder.Entity<IdentityRole>().HasData(roles);

        List<Venue> venues =
        [
            new()
            {
                Id = 1,
                Name = "Grand Conference Hall",
                Description = "A large hall suitable for conferences and big events.",
                Image = null,
                Location = "Downtown Center",
                Capacity = 500
            },
            new()
            {
                Id = 2,
                Name = "Riverside Banquet Hall",
                Description = "Perfect for weddings and receptions with a scenic riverside view.",
                Image = null,
                Location = "Riverside Avenue 12",
                Capacity = 250
            },
            new()
            {
                Id = 3,
                Name = "Skyline Rooftop",
                Description = "An open-air venue with a panoramic city skyline view.",
                Image = null,
                Location = "Highrise Tower, 20th Floor",
                Capacity = 150
            },
            new()
            {
                Id = 4,
                Name = "Greenwood Garden Pavilion",
                Description = "An outdoor garden pavilion ideal for summer parties and casual gatherings.",
                Image = null,
                Location = "Greenwood Park",
                Capacity = 120
            },
            new()
            {
                Id = 5,
                Name = "TechHub Auditorium",
                Description = "Modern auditorium with advanced AV equipment for product launches and seminars.",
                Image = null,
                Location = "Innovation District",
                Capacity = 350
            },
            new()
            {
                Id = 6,
                Name = "Heritage Ballroom",
                Description = "Classic ballroom with chandeliers and vintage decor, perfect for formal galas.",
                Image = null,
                Location = "Old Town Square",
                Capacity = 400
            },
            new()
            {
                Id = 7,
                Name = "Lakeside Retreat",
                Description = "Secluded lakeside lodge for team-building events and weekend retreats.",
                Image = null,
                Location = "Lakeview Road 45",
                Capacity = 80
            },
            new()
            {
                Id = 8,
                Name = "City Art Gallery",
                Description = "Creative space surrounded by art, suitable for exhibitions and cultural events.",
                Image = null,
                Location = "Cultural Avenue 10",
                Capacity = 200
            }
        ];
        modelBuilder.Entity<Venue>().HasData(venues);
    }
}