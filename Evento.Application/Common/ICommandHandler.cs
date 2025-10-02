using Microsoft.AspNetCore.Http;

namespace Evento.Application.Common;

public interface ICommandHandler<TCommand>
{
    Task<IResult> Handle(TCommand command);
}

