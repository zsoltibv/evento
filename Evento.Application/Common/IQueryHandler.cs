using Microsoft.AspNetCore.Http;

namespace Evento.Application.Common;

public interface IQueryHandler<TQuery>
{
    Task<IResult> Handle(TQuery query);
}