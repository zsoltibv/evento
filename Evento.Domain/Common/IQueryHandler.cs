using Microsoft.AspNetCore.Http;

namespace Evento.Domain.Common;

public interface IQueryHandler<TQuery>
{
    Task<IResult> Handle(TQuery query);
}