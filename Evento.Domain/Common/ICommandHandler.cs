using Microsoft.AspNetCore.Http;

namespace Evento.Domain.Common;

public interface ICommandHandler<TCommand>
{
    Task<IResult> Handle(TCommand command);
}

