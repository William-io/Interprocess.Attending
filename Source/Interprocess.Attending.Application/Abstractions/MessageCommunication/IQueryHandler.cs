using Interprocess.Attending.Domain.Abstractions;
using MediatR;

namespace Interprocess.Attending.Application.Abstractions.MessageCommunication;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}