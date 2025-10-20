using Evento.Application.Common;
using Evento.Application.Common.Errors;
using Evento.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Evento.Application.Venues.RequestVenueAdminCommand;

public class RequestVenueAdminCommandHandler(IRoleRequestService service) : ICommandHandler<RequestVenueAdminCommand>
{
    public async Task<IResult> Handle(RequestVenueAdminCommand command)
    {
        var exists = await service.HasPendingRequestAsync(command.UserId, command.VenueId);
        if (exists)
        {
            return Results.BadRequest(RoleRequestErrors.HasPendingRequest);
        }
        
        var result = await service.RequestVenueAdminAsync(command.UserId, command.VenueId);
        return Results.Ok(result);
    }
}