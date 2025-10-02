using Evento.Application.Common;
using Evento.Application.Common.Dto;

namespace Evento.Application.Auth.Login;

public record LoginQuery(LoginDto Dto) : IQuery;