namespace Evento.Common;

public interface IQueryHandler<TQuery>
{
    Task<IResult> Handle(TQuery query);
}