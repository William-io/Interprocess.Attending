﻿using Interprocess.Attending.Domain.Abstractions;
using MediatR;

namespace Interprocess.Attending.Application.Abstractions.MessageCommunication;

public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TReponse> : IRequest<Result<TReponse>>, IBaseCommand
{
}

public interface IBaseCommand
{
}