using Evento.Application.Common.Dto;
using Common_ICommand = Evento.Application.Common.ICommand;
using ICommand = Evento.Application.Common.ICommand;

namespace Evento.Application.Auth.Register;

public record RegisterCommand(RegisterDto Dto) : Common_ICommand;