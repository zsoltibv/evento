using Evento.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Evento.Infrastructure;

public class EventoDbContext(DbContextOptions<EventoDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<VenueAdmin> VenueAdmins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        SeedRoles(modelBuilder);
        SeedAdminUser(modelBuilder);
        SeedVenues(modelBuilder);
        
        modelBuilder.Entity<VenueAdmin>()
            .HasKey(va => new { va.VenueId, va.UserId });
    }

    private static void SeedRoles(ModelBuilder modelBuilder)
    {
        var roles = new List<IdentityRole>
        {
            new() { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new() { Id = "2", Name = "User", NormalizedName = "USER" },
            new() { Id = "3", Name = "VenueAdmin", NormalizedName = "VENUE_ADMIN" }
        };

        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }

    private static void SeedAdminUser(ModelBuilder modelBuilder)
    {
        const string adminId = "00000000-0000-0000-0000-000000000001";
        var adminUser = new AppUser
        {
            Id = adminId,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        var passwordHasher = new PasswordHasher<AppUser>();
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123");

        modelBuilder.Entity<AppUser>().HasData(adminUser);
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = adminId,
                RoleId = "1" 
            }
        );
    }

    private static void SeedVenues(ModelBuilder modelBuilder)
    {
        List<Venue> venues =
        [
            new()
            {
                Id = 1,
                Name = "Grand Conference Hall",
                Description = "A large hall suitable for conferences and big events.",
                ImageUrl =
                    "https://multigrandhotel.com/wp-content/uploads/2014/09/multi_grand_hotel_conference_hall4.jpg",
                Location = "Downtown Center",
                Slug = "grand-conference-hall",
                Capacity = 500
            },
            new()
            {
                Id = 2,
                Name = "Riverside Banquet Hall",
                Description = "Perfect for weddings and receptions with a scenic riverside view.",
                ImageUrl = "https://media.eventective.com/3318884_lg.jpg",
                Location = "Riverside Avenue 12",
                Slug = "riverside-banquet-hall",
                Capacity = 250
            },
            new()
            {
                Id = 3,
                Name = "Skyline Rooftop",
                Description = "An open-air venue with a panoramic city skyline view.",
                ImageUrl = "https://m.dining-out.co.za/ftp/Gallery/10134-15603-32305.jpg",
                Location = "Highrise Tower, 20th Floor",
                Slug = "skyline-rooftop",
                Capacity = 150
            },
            new()
            {
                Id = 4,
                Name = "Greenwood Garden Pavilion",
                Description = "An outdoor garden pavilion ideal for summer parties and casual gatherings.",
                ImageUrl =
                    "https://greenwoodgardens.org/wp-content/uploads/2025/07/Greenwood-Gardens-Summerhouse-1024x682.jpg",
                Location = "Greenwood Park",
                Slug = "greenwood-gardens-summerhouse",
                Capacity = 120
            },
            new()
            {
                Id = 5,
                Name = "TechHub Auditorium",
                Description = "Modern auditorium with advanced AV equipment for product launches and seminars.",
                ImageUrl =
                    "https://cdn.prod.website-files.com/645be0c3de94f82b7aad951a/66daefb089ead127c970d434_Featured-image.jpg",
                Location = "Innovation District",
                Slug = "techhub-auditorium",
                Capacity = 350
            },
            new()
            {
                Id = 6,
                Name = "Heritage Ballroom",
                Description = "Classic ballroom with chandeliers and vintage decor, perfect for formal galas.",
                ImageUrl =
                    "https://partyslate.imgix.net/photos/2100171/photo-7b28f213-a198-4d44-9a12-fa6cc10debf1.jpg?ixlib=js-2.3.2&auto=compress%2Cformat&bg=fff&cs=srgb",
                Location = "Old Town Square",
                Slug = "heritage-ballroom",
                Capacity = 400
            },
            new()
            {
                Id = 7,
                Name = "Lakeside Retreat",
                Description = "Secluded lakeside lodge for team-building events and weekend retreats.",
                ImageUrl =
                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/658805805.jpg?k=6299550817216d7fa8c5f978ec3cf24cf1e78485471833a9a092f3d34d2c99b2&o=&hp=1",
                Location = "Lakeview Road 45",
                Slug = "lakeview-retreat",
                Capacity = 80
            },
            new()
            {
                Id = 8,
                Name = "City Art Gallery",
                Description = "Creative space surrounded by art, suitable for exhibitions and cultural events.",
                ImageUrl =
                    "https://www.thecityofldn.com/wp-content/uploads/2023/05/visitors-at-guildhall-art-gallery-visitthecity.jpg",
                Location = "Cultural Avenue 10",
                Slug = "visitors-at-guildhall-art-gallery",
                Capacity = 200
            }
        ];
        
        modelBuilder.Entity<Venue>().HasData(venues);
    }
}