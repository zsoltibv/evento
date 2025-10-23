using Evento.Application.Services.Interfaces;
using Evento.Domain.Models;
using Evento.Infrastructure.Services.EmailTemplates;
using Evento.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evento.Infrastructure.Services;

public class VenueAdminService(
    EventoDbContext db,
    IEmailTemplateFactory emailTemplateFactory,
    IEmailService emailService) : IVenueAdminService
{
    public async Task AssignVenueAdminAsync(int venueId, string userId)
    {
        if (db.VenueAdmins.Any(a => a.UserId == userId && a.VenueId == venueId))
        {
            return;
        }

        db.VenueAdmins.Add(new VenueAdmin
        {
            VenueId = venueId,
            UserId = userId
        });
        await db.SaveChangesAsync();
    }
    
    public async Task<int[]> GetVenueIdsByUserIdAsync(string userId)
    {
        return await db.VenueAdmins
            .Where(va => va.UserId == userId)
            .Select(va => va.VenueId)
            .ToArrayAsync();
    }

    public async Task<bool> IsUserVenueAdminAsync(string userId, int venueId)
    {
        return await db.VenueAdmins
            .AnyAsync(a => a.UserId == userId && a.VenueId == venueId);
    }

    public async Task SendVenueAdminApprovedEmailAsync(string email, string venueName)
    {
        var message = emailTemplateFactory.CreateEmail<VenueAdminApprovedEmailTemplate>(
            to: email,
            data: venueName);

        await emailService.SendEmailAsync(message);
    }
}