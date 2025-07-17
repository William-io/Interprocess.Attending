using Interprocess.Attending.Domain.Abstractions;
using MediatR;

namespace Interprocess.Attending.Application.Abstractions.MessageCommunication;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
