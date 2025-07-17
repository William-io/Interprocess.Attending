using Interprocess.Attending.Domain.Abstractions;
using MediatR;

namespace Interprocess.Attending.Application.Abstractions.MessageCommunication;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
