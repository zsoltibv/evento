namespace Evento.Common;

public interface ICommandHandler<TCommand>
{
    Task<IResult> Handle(TCommand command);
}

