using Evento.Common;
using Evento.Dto;

namespace Evento.Application.Auth.Register;

public record RegisterCommand(RegisterDto Dto) : ICommand;