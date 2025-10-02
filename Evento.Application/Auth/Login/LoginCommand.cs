using Evento.Application.Common.Dto;
using Evento.Domain.Common;

namespace Evento.Application.Auth.Login;

public record LoginQuery(LoginDto Dto) : IQuery;