using Evento.Application.Common.Dto;
using ICommand = Evento.Application.Common.ICommand;

namespace Evento.Application.Auth.Register;

public record RegisterCommand(RegisterDto Dto) : ICommand;