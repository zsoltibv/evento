using Evento.Application.Common;
using Evento.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Evento.Application.Venues.GetVenueRoles;

public class GetVenueRolesQueryHandler(IRoleRequestService service) : IQueryHandler<GetVenueRolesQuery>
{
    public async Task<IResult> Handle(GetVenueRolesQuery query)
    {
        var result = query.IsAdmin
            ? await service.GetRoleRequestsAsync()
            : await service.GetRoleRequestsAsync(query.UserId);
        
        return Results.Ok(result);
        
    }
}