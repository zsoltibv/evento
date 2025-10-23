using Evento.Application.Common;
using Evento.Application.Common.Errors;
using Evento.Application.Services.Interfaces;
using Evento.Domain.Enums;
using Evento.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Evento.Application.Venues.ApproveVenueAdminCommand;

public class ApproveVenueAdminCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService, IRoleRequestService roleRequestService, IVenueAdminService venueAdminService)
    : ICommandHandler<ApproveVenueAdminCommand>
{
    public async Task<IResult> Handle(ApproveVenueAdminCommand command)
    {
        // Get the role request
        var request = await roleRequestService.GetRoleRequestByIdAsync(command.RoleRequestId);
        if (request == null)
        {
            return Results.NotFound(RoleRequestErrors.NotFound);
        }

        if (request.Status == RequestStatus.Approved)
        {
            return Results.BadRequest(RoleRequestErrors.AlreadyApproved);
        }

        // Update status to approved
        await roleRequestService.UpdateStatusAsync(request.Id, RequestStatus.Approved);

        // Assign venue admin if applicable
        if (request is { VenueId: not null, Venue: not null })
        {
            await venueAdminService.AssignVenueAdminAsync(request.VenueId.Value, request.UserId);
            await venueAdminService.SendVenueAdminApprovedEmailAsync(request.User.Email!, request.Venue.Name);
        }

        // Assign user role
        await userManager.AddToRoleAsync(request.User, request.RoleName);
        var updatedUser = await userManager.FindByIdAsync(request.User.Id);
        
        // Create new token with updated roles
        var token = await tokenService.CreateToken(updatedUser!);
        return Results.Ok(new { Token = token });
    }
}