using Evento.Common;
using Evento.Dto;

namespace Evento.Application.Auth.Login;

public record LoginQuery(LoginDto Dto) : IQuery;